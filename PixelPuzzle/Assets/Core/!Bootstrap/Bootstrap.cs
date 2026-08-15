using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace PixelPuzzle
{
    public class Bootstrap : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;
        private IFieldDataProvider _fieldDataProvider;
        private IImageProvider _imageProvider;
        private IFieldView _fieldView;
        private IBlockSpawner _blockSpawner;
        private IBlockController _blockController;
        private IImageDatabase _imageDatabase;
        private IHighlightService _highlightService;
        private IGlaresFX _glaresFX;
        private IGameController _gameController;
        private IUIController _uiController;
        private IWorldUI _worldUI;
        private ISoundController _soundController;
        private IAnalyticsServiceController _analyticsServiceController;

        [Inject]
        public void Construct(
            ISaveLoadService saveLoadService,
            IFieldDataProvider fieldDataProvider,
            IImageProvider imageProvider,
            IFieldView fieldView,
            IBlockSpawner blockSpawner,
            IBlockController blockController,
            IImageDatabase imageDatabase,
            IHighlightService highlightService,
            IGlaresFX glaresFX,
            IGameController gameController,
            IUIController uiController,
            IWorldUI worldUI,
            ISoundController soundController,
            IAnalyticsServiceController analyticsServiceController
            )
        {
            _saveLoadService = saveLoadService;
            _fieldDataProvider = fieldDataProvider;
            _imageProvider = imageProvider;
            _fieldView = fieldView;
            _blockSpawner = blockSpawner;
            _blockController = blockController;
            _imageDatabase = imageDatabase;
            _highlightService = highlightService;
            _glaresFX = glaresFX;
            _gameController = gameController;
            _uiController = uiController;
            _worldUI = worldUI;
            _soundController = soundController;
            _analyticsServiceController = analyticsServiceController;
        }

        private void Start()
        {
            InitializeAsync(destroyCancellationToken).Forget();
        }

        private async UniTaskVoid InitializeAsync(CancellationToken token)
        {
            _saveLoadService.Initialize();
            GUI_Log.Log("Save load init");

            _fieldDataProvider.Initialize();
            _imageProvider.Initialize();
            _fieldView.Initialize();
            _blockSpawner.Initialize();
            _blockController.Initialize();
            _imageDatabase.Initialize();
            _highlightService.Initialize();
            _glaresFX.Initialize();
            _gameController.Initialize();
            _uiController.Initialize();
            _worldUI.Initialize();
            _soundController.Initialize();
            GUI_Log.Log("Services init");

            await _analyticsServiceController.InitializeAsync(token);
            GUI_Log.Log("Analytics init");

            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                GarbageCollector.CollectIncremental(500000000);
                GUI_Log.Log("GC");
            }

            _fieldView.Hide();
            _worldUI.Hide();
            _gameController.SetState<MenuState>();
            GUI_Log.Log("Game controller start");
        }
    }


}
