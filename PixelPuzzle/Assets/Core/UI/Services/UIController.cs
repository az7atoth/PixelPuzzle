using System.Collections.Generic;
using UnityEngine;

namespace PixelPuzzle
{
    public class UIController : MonoBehaviour, IUIController
    {
        private Dictionary<ViewKey, IUIView> _views;

        public void Initialize()
        {
            CollectViews();
        }

        public void ShowView(ViewKey key)
        {
            if (_views.TryGetValue(key, out IUIView view))
            {
                view.Show();
            }
        }

        public void HideView(ViewKey key)
        {
            if (_views.TryGetValue(key, out IUIView view))
            {
                view.Hide();
            }
        }

        public void HideAll()
        {
            foreach (var view in _views.Values)
            {
                view.Hide();
            }
        }

        private void CollectViews()
        {
            _views = new();

            var views = GetComponentsInChildren<IUIView>();

            foreach (var view in views)
            {
                _views.Add(view.Key, view);
                view.Initialize();
                view.Hide();
            }
        }
    }
}
