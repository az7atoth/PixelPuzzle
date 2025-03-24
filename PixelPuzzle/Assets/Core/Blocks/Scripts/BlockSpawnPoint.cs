using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Threading;
using UnityEngine;

namespace PixelPuzzle
{
    public class BlockSpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform _animationPoint;
        [SerializeField] private float _animationTime = .5f;

        private CancellationTokenSource _cts;
        private TweenerCore<Vector3, Vector3, VectorOptions> _animationTween;

        public void PlaySpawnAnimation(Block block)
        {
            _cts = new();
            PlaySpawnAnimationAsync(block, _cts.Token).Forget();
        }

        private async UniTaskVoid PlaySpawnAnimationAsync(Block block, CancellationToken token)
        {
            Blackboard.SomeAnimationInProgress = true;

            var blockParent = block.transform.parent;

            token.Register(() =>
            {
                _animationTween.Pause();
                block.transform.SetParent(blockParent);
                Blackboard.SomeAnimationInProgress = false;
                return;
            });

            block.transform.SetParent(_animationPoint);
            _animationPoint.localScale = Vector3.zero;

            _animationTween.Rewind();
            await _animationTween.Play()
                .AsyncWaitForCompletion()
                .AsUniTask()
                .AttachExternalCancellation(token);

            _cts?.Dispose();
            _cts = null;

            block.transform.SetParent(blockParent);
            Blackboard.SomeAnimationInProgress = false;
        }

        private void MakeTween()
        {
            _animationTween = _animationPoint.transform
                .DOScale(Vector3.one, _animationTime)
                .ChangeStartValue(Vector3.zero)
                .SetAutoKill(false)
                .Pause();

            _animationPoint.transform.localScale = Vector3.one;
        }

        private void Awake()
        {
            MakeTween();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
