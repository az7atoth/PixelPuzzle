using Az7.Utils.Disposables;
using UnityEngine;
using UniRx;

namespace PixelPuzzle
{
    [RequireComponent(typeof(RectTransform), typeof(UIViewBase))]
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            var view = GetComponent<UIViewBase>();
            view.OnShow.Subscribe(_ => Fit()).AddTo(this);
        }

        private void Fit()
        {
            var min = new Vector2(Screen.safeArea.min.x / Screen.width, Screen.safeArea.min.y / Screen.height);
            var max = new Vector2(Screen.safeArea.max.x / Screen.width, Screen.safeArea.max.y / Screen.height);

            _rectTransform.anchorMin = min;
            _rectTransform.anchorMax = max;
        }
    }
}
