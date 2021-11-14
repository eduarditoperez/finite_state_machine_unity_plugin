using Fsm.State.Transition;

namespace Fsm.State
{
    /// <summary>
    /// Place holder transition
    /// </summary>
    public class NoopTransition : FsmTransition
    {
        public void Init(string transitionName, State nextState)
        {
            TransitionName = transitionName;
            NextState = nextState;
            IsValid = false;
        }
    }
}