using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class SoundFX_Panel : UIViewBase
    {
        public override ViewKey Key => ViewKey.SFX_Panel;

        [SerializeField] private Button _normalButton;
        [SerializeField] private Button _mutedButton;

        private ISoundController _soundController;

        [Inject]
        public void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        public override void Initialize()
        {
            _normalButton.onClick.AsObservable().Subscribe(_ =>
            {
                ToggleMute();
            }).AddTo(this);

            _mutedButton.onClick.AsObservable().Subscribe(_ =>
            {
                ToggleMute();
            }).AddTo(this);

            _mutedButton.gameObject.SetActive(false);
        }

        public void ToggleMute()
        {
            if (_soundController.IsMuted)
            {
                _soundController.SetVolume(1f);
                _normalButton.gameObject.SetActive(true);
                _mutedButton.gameObject.SetActive(false);

                _soundController.Play(SoundKey.ButtonClick);
            }
            else
            {
                _soundController.SetVolume(0f);
                _normalButton.gameObject.SetActive(false);
                _mutedButton.gameObject.SetActive(true);
            }
        }
    }
}
