using Az7.Utils.Disposables;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace PixelPuzzle
{
    public class RunningState : GameStateBase
    {
        private GameConfig _gameConfig;
        private IGameController _gameController;
        private IImageProvider _imageProvider;
        private IFieldDataProvider _fieldDataProvider;
        private IFieldView _fieldView;
        private IBlockController _blockController;
        private IBlockSpawner _blockSpawner;
        private IUIController _uIController;
        private IWorldUI _worldUI;
        private ICurtain _curtain;
        private ISoundController _soundController;
        private IAnalyticsServiceController _analyticsServiceController;

        private float _startLevelTime;
        private CancellationTokenSource _cts;
        private DisposableTracker _disposableTracker = new();

        [Inject]
        public void Construct(
            GameConfig gameConfig,
            IGameController gameController,
            IImageProvider imageProvider,
            IFieldDataProvider fieldDataProvider,
            IFieldView fieldView,
            IBlockController blockController,
            IBlockSpawner blockSpawner,
            IUIController uIController,
            IWorldUI worldUI,
            ICurtain curtain,
            ISoundController soundController,
            IAnalyticsServiceController analyticsServiceController
            )
        {
            _gameConfig = gameConfig;
            _gameController = gameController;
            _imageProvider = imageProvider;
            _fieldDataProvider = fieldDataProvider;
            _fieldView = fieldView;
            _blockController = blockController;
            _blockSpawner = blockSpawner;
            _uIController = uIController;
            _worldUI = worldUI;
            _curtain = curtain;
            _soundController = soundController;
            _analyticsServiceController = analyticsServiceController;
        }

        public override void OnEnter()
        {
            _fieldView.Show();
            _worldUI.Show();

            _cts = new();
            StartGameAsync(_cts.Token).Forget();

            EventBus.UI_OnMainMenuPressed.Subscribe(_ =>
            {
                LeaveGame();
            }).AddTo(_disposableTracker);

            EventBus.OnPuzzleSolved.Subscribe(_ =>
            {
                Blackboard.GameInProgress = false;
                AnalyticsOnPuzzleSolved();
                _soundController.Play(SoundKey.PuzzleSolved);
                _uIController.ShowView(ViewKey.EndGameMenu);
            }).AddTo(_disposableTracker);

            EventBus.UI_OnStartGamePressed.Subscribe(_ =>
            {
                _cts = new();
                StartNextLevelAsync(_cts.Token).Forget();
            }).AddTo(_disposableTracker);
        }

        public override void OnExit()
        {
            Blackboard.GameInProgress = false;

            _blockSpawner.ReturnAll();
            _fieldView.Hide();
            _worldUI.Hide();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _disposableTracker.Dispose();
        }

        private async UniTask StartGameAsync(CancellationToken token)
        {
            token.Register(() => { Blackboard.SomeAnimationInProgress = false; return; });

            Blackboard.SomeAnimationInProgress = true;
            Blackboard.AvailableHints.Value = _gameConfig.MaxAvailableHints;

            _fieldDataProvider.PrepareNewField();
            _fieldView.PrepareNewField();

            foreach (var data in _fieldDataProvider.Cells)
            {
                if (data.BlockID < 0)
                {
                    _fieldView.OpenCell(data.ID);
                }
            }

            for (int i = 0; i < _gameConfig.OnStartRevealedBlocks; i++)
            {
                _blockController.RevealRandomBlock();
            }

            _blockController.Start();

            var waitTime = Random.Range(.5f, 1.2f);
            await UniTask.WaitForSeconds(waitTime, cancellationToken: token);

            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                GarbageCollector.CollectIncremental(100000000);
            }

            _curtain.HideImmidiate();

            _startLevelTime = Time.realtimeSinceStartup;

            Blackboard.SomeAnimationInProgress = false;
            Blackboard.GameInProgress = true;
        }

        private void LeaveGame()
        {
            _uIController.HideAll();
            _gameController.SetState<MenuState>();
        }

        private async UniTask StartNextLevelAsync(CancellationToken token)
        {
            _curtain.ShowImmidiate();

            _uIController.HideAll();
            _imageProvider.LoadNextImage();

            await StartGameAsync(token);
        }

        private void AnalyticsOnPuzzleSolved()
        {
            var solvingTime = Time.realtimeSinceStartup - _startLevelTime;
            var imageId = _imageProvider.GetImageData().ID;
            var hints_used = _gameConfig.MaxAvailableHints - Blackboard.AvailableHints.Value;

            _analyticsServiceController.SendOnPuzzleSolved(imageId, solvingTime, hints_used);
        }

        public class Factory : PlaceholderFactory<RunningState> { }
    }
}
