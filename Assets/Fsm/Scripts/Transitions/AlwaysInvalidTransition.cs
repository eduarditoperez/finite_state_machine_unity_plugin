namespace Fsm.State.Transition
{
    public class AlwaysInvalidTransition : FsmTransition
    {
        public override string TransitionName => typeof(AlwaysInvalidTransition).Name;
        public override FsmStateBase NextState => _nextState;

        private FsmStateBase _nextState;

        public AlwaysInvalidTransition(FsmStateBase nextState)
        {
            _nextState = nextState;
        }

        public override bool IsValid() => false;
    }
}