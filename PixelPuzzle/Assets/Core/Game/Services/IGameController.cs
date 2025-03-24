namespace PixelPuzzle
{
    public interface IGameController
    {
        public void Initialize();
        public void SetState<T>() where T : GameStateBase;
    }
}
