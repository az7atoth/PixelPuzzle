using System;

namespace Az7.Core.StateMachine
{
    public abstract class StateMachineBase<T> where T : IState
    {
        public event Action<T, T> OnStateChanges;

        public T CurrentState { get; private set; }

        public void ChangeState(T newState)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }

            OnStateChanges?.Invoke(CurrentState, newState);
            CurrentState = newState;

            if (CurrentState != null)
            {
                CurrentState.OnEnter();
            }
        }

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.OnUpdate();
            }
        }
    }
}
