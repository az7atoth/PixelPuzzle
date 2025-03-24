namespace Az7.Core.StateMachine
{
    public interface IState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }
}
