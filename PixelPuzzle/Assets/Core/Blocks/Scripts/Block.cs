using Az7.Utils.Pool;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace PixelPuzzle
{
    public class Block : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private const float AUTO_MOVE_SPEED = 5f;

        public int SpawnIndex { get; private set; }
        public BlockData BlockData { get; private set; }

        [SerializeField] private PoolableObject _poolableObject;
        [SerializeField] private Transform _collider;
        [SerializeField] private FieldCell[] _fieldCells;

        private ISoundController _soundController;

        private Vector3 _startPosition;
        private Vector3 _cursorOffset;
        private bool _dragging;

        private CancellationTokenSource _cts;
        private TweenerCore<Vector3, Vector3, VectorOptions> _autoMoveTween;

        [Inject]
        public void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        public void Setup(int spawnIndex, ImageData imageData, BlockData blockData)
        {
            SpawnIndex = spawnIndex;

            BlockData = blockData;

            for (int i = 0; i < BlockData.CellPositions.Length; i++)
            {
                var gridPosition = BlockData.CellPositions[i] + BlockData.OriginPosition;
                _fieldCells[i].SetPart(imageData.GetPart(gridPosition));

                var position = new Vector3(BlockData.CellPositions[i].x, BlockData.CellPositions[i].y, 0);
                _fieldCells[i].transform.localPosition = position;
                _fieldCells[i].SetEnabled(true);
            }

            var offset = GetSpawnOffset();

            _collider.localPosition = offset;
            _startPosition = transform.position - offset;
            transform.position = _startPosition;

            SetSortingOrder(SpawnIndex + 1);
        }

        public void AutoPlace()
        {
            _cts = new();
            AutoPlaceAsync(_cts.Token).Forget();
        }

        public void Return()
        {
            EventBus.OnBlockReturned.Execute(this);
            _poolableObject.Return();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Blackboard.SomeAnimationInProgress || _dragging) return;

            _dragging = true;

            _soundController.Play(SoundKey.PuzzlePartPicked, 1.2f);

            var cursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);

            _cursorOffset = transform.position - cursorPosition;

            //set additional offset for platforms with touch input
            if (Application.platform != RuntimePlatform.WindowsPlayer
                && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                _cursorOffset += Vector3.up * 3f;
            }

            SetSortingOrder(SpawnIndex + 100);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Blackboard.SomeAnimationInProgress || !_dragging) return;

            var cursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            var position = new Vector3(cursorPosition.x + _cursorOffset.x, cursorPosition.y + _cursorOffset.y, _startPosition.z);
            transform.position = position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Blackboard.SomeAnimationInProgress || !_dragging) return;

            _dragging = false;

            var position = GetIntPosition();

            if (position == BlockData.OriginPosition)
            {
                _soundController.Play(SoundKey.PuzzlePartPlaced);
                EventBus.OnBlockPlaced.Execute(this);
            }
            else
            {
                _soundController.Play(SoundKey.PuzzlePartPicked, .8f);
                transform.position = _startPosition;
                SetSortingOrder(SpawnIndex + 1);
            }
        }

        private Vector2Int GetIntPosition()
        {
            var x = Mathf.RoundToInt(transform.position.x);
            var y = Mathf.RoundToInt(transform.position.y);
            return new Vector2Int(x, y);
        }

        private void SetSortingOrder(int order)
        {
            for (int i = 0; i < _fieldCells.Length; i++)
            {
                _fieldCells[i].SetSortingOrder(order);
            }
        }

        private Vector3 GetSpawnOffset()
        {
            var x = 0;
            var y = 0;

            for (int i = 0; i < BlockData.CellPositions.Length; i++)
            {
                if (BlockData.CellPositions[i].x > x)
                {
                    x = BlockData.CellPositions[i].x;
                }

                if (BlockData.CellPositions[i].y > y)
                {
                    y = BlockData.CellPositions[i].y;
                }
            }

            return new Vector3(x + 1, y + 1, 0f) * .5f;
        }

        private async UniTaskVoid AutoPlaceAsync(CancellationToken token)
        {
            Blackboard.SomeAnimationInProgress = true;

            SetSortingOrder(SpawnIndex + 100);

            token.Register(() =>
            {
                _autoMoveTween.Pause();
                Blackboard.SomeAnimationInProgress = false;
                return;
            });

            var start = transform.position;
            var end = new Vector3(BlockData.OriginPosition.x, BlockData.OriginPosition.y, transform.position.z);

            _autoMoveTween.ChangeStartValue(start);
            _autoMoveTween.ChangeEndValue(end);
            _autoMoveTween.Rewind();

            await _autoMoveTween.Play().AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(token);

            Blackboard.SomeAnimationInProgress = false;

            _soundController.Play(SoundKey.PuzzlePartPlaced);
            EventBus.OnBlockPlaced.Execute(this);
        }

        private void Awake()
        {
            MakeTween();
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void MakeTween()
        {
            _autoMoveTween = transform
                .DOMove(Vector3.zero, AUTO_MOVE_SPEED)
                .SetSpeedBased(true)
                .SetAutoKill(false)
                .SetEase(Ease.InOutCubic)
                .Pause();
        }
    }
}
