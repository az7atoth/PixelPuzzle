using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class EndGameMenu : UIViewBase
    {
        public override ViewKey Key => ViewKey.EndGameMenu;

        [SerializeField] private Transform _content;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private float _animationTime = 1f;

        private ISoundController _soundController;

        private Tweener _scaleAnimationTween;
        private CancellationTokenSource _cts;

        [Inject]
        public void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        public override void Initialize()
        {
            MakeTween();

            _nextButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    _soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnStartGamePressed.Execute();
                }).AddTo(this);

            _menuButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    _soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnMainMenuPressed.Execute();
                }).AddTo(this);
        }

        public override void Show()
        {
            base.Show();

            _cts = new();
            ShowViewAsync(_cts.Token).Forget();
        }

        public override void Hide()
        {
            base.Hide();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTaskVoid ShowViewAsync(CancellationToken token)
        {
            Blackboard.SomeAnimationInProgress = true;

            token.Register(() =>
            {
                _scaleAnimationTween.Pause();
                Blackboard.SomeAnimationInProgress = false;
                return;
            });

            _scaleAnimationTween.Rewind();

            await _scaleAnimationTween.Play()
                .AsyncWaitForCompletion()
                .AsUniTask()
                .AttachExternalCancellation(token);

            Blackboard.SomeAnimationInProgress = false;
        }

        private void MakeTween()
        {
            _scaleAnimationTween = _content
                .DOScale(Vector3.one, _animationTime)
                .ChangeStartValue(Vector3.zero)
                .SetAutoKill(false)
                .SetEase(Ease.InOutCubic)
                .Pause();
        }
    }
}
