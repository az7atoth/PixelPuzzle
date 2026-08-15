using UniRx;
using UnityEngine;

namespace PixelPuzzle
{
    public abstract class UIViewBase : MonoBehaviour, IUIView
    {
        public ReactiveCommand OnShow { get; private set; } = new();
        public ReactiveCommand OnHide { get; private set; } = new();

        public virtual ViewKey Key { get; protected set; }

        [SerializeField] protected CanvasGroup _canvasGroup;

        public virtual void Initialize() { }

        public virtual void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            OnShow.Execute();
        }

        public virtual void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            OnHide.Execute();
        }
    }

    public interface IUIView
    {
        public ViewKey Key { get; }
        public void Initialize();
        public void Show();
        public void Hide();
    }
}
