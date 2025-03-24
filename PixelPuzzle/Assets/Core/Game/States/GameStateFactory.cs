namespace PixelPuzzle
{
    public class GameStateFactory : IGameStateFactory
    {
        private MenuState.Factory _menuStateFactory;
        private RunningState.Factory _runningStateFactory;

        public GameStateFactory(
            MenuState.Factory menuStateFactory,
            RunningState.Factory runningStateFactory)
        {
            _menuStateFactory = menuStateFactory;
            _runningStateFactory = runningStateFactory;
        }

        public T Create<T>() where T : GameStateBase
        {
            if (typeof(T).IsEquivalentTo(typeof(MenuState)))
            {
                return _menuStateFactory.Create() as T;
            }
            else
            if (typeof(T).IsEquivalentTo(typeof(RunningState)))
            {
                return _runningStateFactory.Create() as T;
            }
            else
            {
                return null;
            }
        }
    }
}
