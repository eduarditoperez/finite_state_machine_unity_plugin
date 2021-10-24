using Fsm.State.Transition;

namespace Fsm.State
{
    /// <summary>
    /// Place holder transition
    /// </summary>
    public class NoopTransition : FsmTransition
    {
        private string _transitionName;
        private FsmState _nextState;

        public string TransitionName => _transitionName;
        public FsmState NextState => _nextState;

        public NoopTransition(string transitionName, FsmState nextState)
        {
            _transitionName = transitionName;
            _nextState = nextState;
        }

        public bool IsValid()
        {
            return true;
        }
    }
}