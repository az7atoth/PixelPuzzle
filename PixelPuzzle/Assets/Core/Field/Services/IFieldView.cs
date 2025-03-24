namespace PixelPuzzle
{
    public interface IFieldView
    {
        public void Initialize();
        public void PrepareNewField();
        public void OpenCell(int id);
        public void ClearAll();
        public void Show();
        public void Hide();
    }
}
