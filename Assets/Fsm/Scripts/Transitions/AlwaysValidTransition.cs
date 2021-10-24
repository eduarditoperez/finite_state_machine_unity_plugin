namespace Fsm.State.Transition
{
    public class AlwaysValidTransition : FsmTransition
    {
        public string TransitionName => typeof(AlwaysValidTransition).Name;

        public FsmState NextState => _nextState;

        private FsmState _nextState;

        public AlwaysValidTransition(FsmState nextState)
        {
            _nextState = nextState;
        }

        public bool IsValid() => true;

    }
}
