using Az7.Core.StateMachine;

namespace PixelPuzzle
{
    public abstract class GameStateBase : IState
    {
        public abstract void OnEnter();

        public virtual void OnExit() { }

        public virtual void OnUpdate() { }
    }
}
