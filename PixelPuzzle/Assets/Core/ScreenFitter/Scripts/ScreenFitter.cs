using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

namespace PixelPuzzle
{
    public class ScreenFitter : MonoBehaviour
    {
        public ReactiveCommand OnFit { get; private set; } = new();

        [SerializeField] private float _referenceWidth;
        [SerializeField] private float _referenceHeight;
        [SerializeField] private float _referenceCameraSize;
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private float _updateInterval = .2f;

        private float _screenRatio;
        //private float _referenceCameraSize;
        private float _referenceScreenRatio;
        private float _targetCameraSize;

        private float _storedWidth;
        private float _storedHeight;

        private void Awake()
        {
            _referenceScreenRatio = _referenceWidth / _referenceHeight;
            _storedWidth = _referenceWidth;
            _storedHeight = _referenceHeight;

            FitScreenAsync(destroyCancellationToken).Forget();
        }

        private async UniTask FitScreenAsync(CancellationToken token)
        {
            await UniTask.WaitForFixedUpdate(token);

            while (!token.IsCancellationRequested)
            {
                if (Screen.width != _storedWidth || Screen.height != _storedHeight)
                {
                    _storedWidth = Screen.width;
                    _storedHeight = Screen.height;

                    _screenRatio = Screen.width / (float)Screen.height;
                    _targetCameraSize = _referenceCameraSize * _referenceScreenRatio / _screenRatio;

                    if (_screenRatio < _referenceScreenRatio)
                    {
                        _gameCamera.orthographicSize = _targetCameraSize;
                    }
                    else
                    {
                        _gameCamera.orthographicSize = _referenceCameraSize;
                    }

                    OnFit.Execute();
                }

                await UniTask.WaitForSeconds(_updateInterval, cancellationToken: token);
            }
        }
    } 
}
