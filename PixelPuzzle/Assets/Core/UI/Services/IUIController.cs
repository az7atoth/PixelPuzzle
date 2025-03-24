namespace PixelPuzzle
{
    public interface IUIController
    {
        public void Initialize();
        public void ShowView(ViewKey key);
        public void HideView(ViewKey key);
        public void HideAll();
    }
}
