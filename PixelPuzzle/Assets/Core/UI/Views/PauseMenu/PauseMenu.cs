using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class PauseMenu : UIViewBase
    {
        public override ViewKey Key => ViewKey.PauseMenu;

        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private IUIController _uIcontroller;
        private ISoundController _soundController;

        [Inject]
        public void Construct(ISoundController soundController, IUIController uIController)
        {
            _soundController = soundController;
            _uIcontroller = uIController;
        }

        public override void Initialize()
        {
            _yesButton.onClick.AsObservable().Subscribe(_ =>
            {
                _soundController.Play(SoundKey.ButtonClick);
                EventBus.UI_OnMainMenuPressed.Execute();
            }).AddTo(this);

            _noButton.onClick.AsObservable().Subscribe(_ =>
            {
                _soundController.Play(SoundKey.ButtonClick);
                Hide();
            }).AddTo(this);
        }

        public override void Show()
        {
            base.Show();

            _uIcontroller.ShowView(ViewKey.SFX_Panel);
        }

        public override void Hide()
        {
            base.Hide();

            _uIcontroller.HideView(ViewKey.SFX_Panel);
        }
    }
}
