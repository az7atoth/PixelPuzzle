using UniRx;
using UnityEngine;

namespace PixelPuzzle
{
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        public ReactiveCommand OnAnimationStopped { get; private set; } = new();

        [SerializeField] private float _frameDelay = .2f;
        [SerializeField] private SpriteRenderer _target;
        [SerializeField] private Sprite[] _frames;
        [SerializeField] private bool _playOnce;

        private float _timer;
        private int _frameIndex;
        private bool _stopped;

        private void Update()
        {
            if (_stopped) return;

            _timer += Time.deltaTime;

            if (_timer >= _frameDelay)
            {
                _timer = 0f;

                _frameIndex++;

                if (_frameIndex >= _frames.Length)
                {
                    _frameIndex = 0;

                    if (_playOnce)
                    {
                        _stopped = true;
                        OnAnimationStopped.Execute();
                        return;
                    }
                }

                _target.sprite = _frames[_frameIndex];
            }
        }

        private void OnEnable()
        {
            _stopped = false;

            _timer = 0f;

            _frameIndex = 0;
            _target.sprite = _frames[_frameIndex];
        }
    }
}
