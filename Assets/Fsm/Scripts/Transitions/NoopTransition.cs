using Fsm.State.Transition;

namespace Fsm.State
{
    /// <summary>
    /// Place holder transition
    /// </summary>
    public class NoopTransition : FsmTransition
    {
        public void Init(string transitionName, FsmStateBase nextState)
        {
            TransitionName = transitionName;
            NextState = nextState;
            IsValid = false;
        }
    }
}