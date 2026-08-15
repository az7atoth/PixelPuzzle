using Az7.Utils.Disposables;
using Cysharp.Threading.Tasks;
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

            _curtain.HideImmidiate();

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
                ToRunningState();
            }).AddTo(_disposableTracker);

            EventBus.UI_OnLevelChoose.Subscribe(id =>
            {
                _soundController.Play(SoundKey.ButtonClick);
                _imageProvider.LoadImage(id);
                ToRunningState();
            }).AddTo(_disposableTracker);
        }

        public override void OnExit()
        {
            _disposableTracker.Dispose();
        }

        private void ToRunningState()
        {
            _curtain.ShowImmidiate();

            _uIController.HideAll();
            _gameController.SetState<RunningState>();
        }

        public class Factory : PlaceholderFactory<MenuState> { }
    }
}
