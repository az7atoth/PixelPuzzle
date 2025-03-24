using Az7.Utils.Disposables;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;
using UniRx;

namespace PixelPuzzle
{
    public class MenuState : GameStateBase
    {
        private IGameController _gameController;
        private IImageProvider _imageProvider;
        private IUIController _uIController;
        private ISoundController _soundController;
        private ICurtain _curtain;

        private CancellationTokenSource _cts;
        private DisposableTracker _disposableTracker = new();

        [Inject]
        public void Construct(
            IGameController gameController,
            IImageProvider imageProvider,
            IUIController uIController,
            ISoundController soundController,
            ICurtain curtain
            )
        {
            _gameController = gameController;
            _imageProvider = imageProvider;
            _uIController = uIController;
            _soundController = soundController;
            _curtain = curtain;
        }

        public override void OnEnter()
        {
            _uIController.ShowView(ViewKey.MainMenu);
            _uIController.ShowView(ViewKey.SFX_Panel);

            _cts = new();
            _curtain.HideAsync(_cts.Token).Forget();

            EventBus.UI_OnLevelMenuPressed.Subscribe(_ =>
            {
                _uIController.HideAll();
                _uIController.ShowView(ViewKey.LevelsMenu);
            }).AddTo(_disposableTracker);

            EventBus.UI_OnCancellationButtonPressed.Subscribe(_ =>
            {
                _uIController.HideAll();
                _uIController.ShowView(ViewKey.MainMenu);
                _uIController.ShowView(ViewKey.SFX_Panel);
            }).AddTo(_disposableTracker);

            EventBus.UI_OnStartGamePressed.Subscribe(_ =>
            {
                _imageProvider.LoadNextImage();
                _cts = new();
                ToRunningAsync(_cts.Token).Forget();
            }).AddTo(_disposableTracker);

            EventBus.UI_OnLevelChoose.Subscribe(id =>
            {
                _soundController.Play(SoundKey.ButtonClick);
                _imageProvider.LoadImage(id);
                _cts = new();
                ToRunningAsync(_cts.Token).Forget();
            }).AddTo(_disposableTracker);
        }

        public override void OnExit()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _disposableTracker.Dispose();
        }

        private async UniTaskVoid ToRunningAsync(CancellationToken token)
        {
            await _curtain.ShowAsync(token);
            _uIController.HideAll();
            _gameController.SetState<RunningState>();
        }

        public class Factory : PlaceholderFactory<MenuState> { }
    }
}
