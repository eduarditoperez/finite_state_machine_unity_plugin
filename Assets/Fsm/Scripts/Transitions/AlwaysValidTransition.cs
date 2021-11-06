namespace Fsm.State.Transition
{
    public class AlwaysValidTransition : FsmTransition
    {
        public override string TransitionName => typeof(AlwaysValidTransition).Name;

        public override FsmStateBase NextState => _nextState;

        private FsmStateBase _nextState;

        public AlwaysValidTransition(FsmStateBase nextState)
        {
            _nextState = nextState;
        }

        public override bool IsValid() => true;

    }
}
