namespace Fsm.State.Transition
{
    public class AlwaysInvalidTransition : FsmTransition
    {
        public AlwaysInvalidTransition(FsmStateBase nextState) 
            : base (typeof(AlwaysInvalidTransition).Name, nextState, false) {}
    }
}