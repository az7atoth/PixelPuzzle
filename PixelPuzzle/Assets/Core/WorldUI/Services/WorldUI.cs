using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class WorldUI : MonoBehaviour, IWorldUI
    {
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private TMP_Text _hintsText;

        IBlockController _blockController;
        IUIController _uiController;
        ISoundController _soundController;
        GameConfig _gameConfig;

        [Inject]
        public void Construct(
            IBlockController blockController,
            IUIController uIController,
            ISoundController soundController,
            GameConfig gameConfig)
        {
            _blockController = blockController;
            _uiController = uIController;
            _soundController = soundController;
            _gameConfig = gameConfig;
        }

        public void Initialize()
        {
            _hintButton.onClick.AsObservable().Subscribe(_ => OnHintsButtonClicked()).AddTo(this);
            _menuButton.onClick.AsObservable().Subscribe(_ => OnMenuButtonClicked()).AddTo(this);

            Blackboard.AvailableHints.Subscribe(value =>
            {
                _hintsText.text = $"{value}/{_gameConfig.MaxAvailableHints}";
            }).AddTo(this);
        }

        public void Show()
        {
            _hintButton.gameObject.SetActive(true);
            _menuButton.gameObject.SetActive(true);
            _hintsText.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _hintButton.gameObject.SetActive(false);
            _menuButton.gameObject.SetActive(false);
            _hintsText.gameObject.SetActive(false);
        }

        private void OnHintsButtonClicked()
        {
            if (Blackboard.SomeAnimationInProgress
                || !Blackboard.GameInProgress)
            {
                return;
            }

            if (Blackboard.AvailableHints.Value > 0)
            {
                Blackboard.AvailableHints.Value--;
                _blockController.AutoPlaceRandomBlock();
                _soundController.Play(SoundKey.HintButton);
            }
            else
            {
                _soundController.Play(SoundKey.ButtonClick);
            }
        }

        private void OnMenuButtonClicked()
        {
            if (Blackboard.SomeAnimationInProgress) return;

            _uiController.ShowView(ViewKey.PauseMenu);
            _soundController.Play(SoundKey.ButtonClick);
        }

    }
}
