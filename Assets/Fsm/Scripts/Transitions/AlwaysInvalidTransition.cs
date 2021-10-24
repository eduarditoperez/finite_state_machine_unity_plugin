namespace Fsm.State.Transition
{
    public class AlwaysInvalidTransition : FsmTransition
    {
        public string TransitionName => typeof(AlwaysInvalidTransition).Name;
        public FsmState NextState => _nextState;

        private FsmState _nextState;

        public AlwaysInvalidTransition(FsmState nextState)
        {
            _nextState = nextState;
        }

        public bool IsValid() => false;
    }
}