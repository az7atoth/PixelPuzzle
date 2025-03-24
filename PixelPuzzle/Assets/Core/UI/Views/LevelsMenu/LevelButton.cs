using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PixelPuzzle
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _defaultButtonView;
        [SerializeField] private Button _solvedButtonView;
        [SerializeField] private TMP_Text _buttonText;

        //private ISoundController _soundController;

        //[Inject]
        //public void Construct(ISoundController soundController)
        //{
        //    _soundController = soundController;
        //}

        public int ID { get; private set; }

        public void Setup(int id, bool solved)
        {
            ID = id;

            _buttonText.text = (id + 1).ToString();

            UpdateButton(solved);
        }

        public void UpdateButton(bool solved)
        {
            if (solved)
            {
                _defaultButtonView.gameObject.SetActive(false);
                _solvedButtonView.gameObject.SetActive(true);
            }
            else
            {
                _defaultButtonView.gameObject.SetActive(true);
                _solvedButtonView.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            _defaultButtonView.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    //_soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnLevelChoose.Execute(ID);
                }).AddTo(this);

            _solvedButtonView.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    //_soundController.Play(SoundKey.ButtonClick);
                    EventBus.UI_OnLevelChoose.Execute(ID);
                }).AddTo(this);
        }

    }
}
