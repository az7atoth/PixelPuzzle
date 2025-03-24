using Zenject;

namespace PixelPuzzle
{
    public class GameController : IGameController
    {
        private IGameStateFactory _gameStateFactory;

        private GameStateMachine _gameStateMachine;
        private MenuState _menuState;
        private RunningState _runningState;

        [Inject]
        public void Construct(IGameStateFactory gameStateFactory)
        {
            _gameStateFactory = gameStateFactory;
        }

        public void Initialize()
        {
            _gameStateMachine = new();
            _menuState = _gameStateFactory.Create<MenuState>();
            _runningState = _gameStateFactory.Create<RunningState>();
        }

        public void SetState<T>() where T : GameStateBase
        {
            if (typeof(T).IsEquivalentTo(typeof(MenuState)))
            {
                if (_gameStateMachine.CurrentState == _menuState) return;

                _gameStateMachine.ChangeState(_menuState);
            }
            else
            if (typeof(T).IsEquivalentTo(typeof(RunningState)))
            {
                if (_gameStateMachine.CurrentState == _runningState) return;

                _gameStateMachine.ChangeState(_runningState);
            }
        }
    }
}
