using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class LevelsMenu : UIViewBase
    {
        public override ViewKey Key => ViewKey.LevelsMenu;

        [SerializeField] private LevelButton _levelButtonPrefab;
        [SerializeField] private Transform _levelsContent;
        [SerializeField] private Button _cancelButton;

        private IImageDatabase _imageDatabase;
        private ISaveLoadService _saveLoadService;
        private ISoundController _soundController;

        private Dictionary<int, LevelButton> _buttons;

        [Inject]
        public void Construct(IImageDatabase imageDatabase, ISaveLoadService saveLoadService, ISoundController soundController)
        {
            _imageDatabase = imageDatabase;
            _saveLoadService = saveLoadService;
            _soundController = soundController;
        }

        public override void Initialize()
        {
            _cancelButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    _soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnCancellationButtonPressed.Execute();
                }).AddTo(this);

            MakeButtons();
        }

        public override void Show()
        {
            base.Show();

            UpdateButtons();
        }

        private void MakeButtons()
        {
            ClearButtons();

            _buttons = new Dictionary<int, LevelButton>();

            foreach (var item in _imageDatabase.Images)
            {
                var id = item.Key;

                var button = Instantiate(_levelButtonPrefab, _levelsContent);
                var solved = _saveLoadService.SaveData.SolvedImagesIDs.Contains(id);

                button.Setup(id, solved);
                _buttons.Add(id, button);
            }
        }

        private void UpdateButtons()
        {
            foreach (var item in _imageDatabase.Images)
            {
                var id = item.Key;

                var button = _buttons[id];

                var solved = _saveLoadService.SaveData.SolvedImagesIDs.Contains(id);

                button.UpdateButton(solved);
            }
        }

        private void ClearButtons()
        {
            var buttons = _levelsContent.GetComponentsInChildren<LevelButton>();

            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
        }
    }
}
