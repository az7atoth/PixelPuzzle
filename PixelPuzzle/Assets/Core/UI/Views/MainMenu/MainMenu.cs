using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class MainMenu : UIViewBase
    {
        public override ViewKey Key => ViewKey.MainMenu;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _levelsMenuButton;
        [SerializeField] private Button _exitMenuButton;

        private ISoundController _soundController;

        [Inject]
        public void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        public override void Initialize()
        {
            _startGameButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    _soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnStartGamePressed.Execute();
                }).AddTo(this);

            _levelsMenuButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    _soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnLevelMenuPressed.Execute();
                }).AddTo(this);

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                _exitMenuButton.gameObject.SetActive(true);

                _exitMenuButton.onClick.AsObservable()
                    .Subscribe(_ => { Application.Quit(); }).AddTo(this);
            }
        }
    }
}
