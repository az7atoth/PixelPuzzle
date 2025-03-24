namespace PixelPuzzle
{
    public interface IGameStateFactory
    {
        public T Create<T>() where T : GameStateBase;
    }
}
