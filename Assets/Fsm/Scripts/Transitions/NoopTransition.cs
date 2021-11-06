using Fsm.State.Transition;

namespace Fsm.State
{
    /// <summary>
    /// Place holder transition
    /// </summary>
    public class NoopTransition : FsmTransition
    {
        private string _transitionName;
        private FsmStateBase _nextState;

        public override string TransitionName => _transitionName;
        public override FsmStateBase NextState => _nextState;

        public NoopTransition(string transitionName, FsmStateBase nextState)
        {
            _transitionName = transitionName;
            _nextState = nextState;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}