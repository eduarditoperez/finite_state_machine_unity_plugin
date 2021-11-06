using Fsm.State.Transition;

namespace Fsm.State
{
    /// <summary>
    /// Place holder transition
    /// </summary>
    public class NoopTransition : FsmTransition
    {
        public NoopTransition(string transitionName, FsmStateBase nextState)
                : base(transitionName, nextState, true) { }
    }
}