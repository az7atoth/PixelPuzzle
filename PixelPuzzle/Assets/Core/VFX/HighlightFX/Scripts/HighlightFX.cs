using Az7.Utils.Pool;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

namespace PixelPuzzle
{
    public class HighlightFX : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private PoolableObject _poolableObject;
        [SerializeField] private float _animationTime;
        [SerializeField] private Color _color;

        private CancellationTokenSource _cts;
        private Tweener _colorTween;

        public void Play(Vector3[] positions)
        {
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);

            _cts = new CancellationTokenSource();
            PlayAsync(_cts.Token).Forget();
        }

        public void Return()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _poolableObject.Return();
        }

        private async UniTaskVoid PlayAsync(CancellationToken token)
        {
            token.Register(() =>
            {
                _colorTween.Pause();
                return;
            });

            _colorTween.Rewind();

            await _colorTween.Play()
                .AsyncWaitForCompletion()
                .AsUniTask()
                .AttachExternalCancellation(token);

            Return();
        }

        private void MakeTween()
        {
            Color2 start = new Color2(_color, _color);
            Color2 end = new Color2(Color.clear, Color.clear);

            _colorTween = _lineRenderer.DOColor(start, end, _animationTime)
                .SetAutoKill(false)
                .Pause();
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
