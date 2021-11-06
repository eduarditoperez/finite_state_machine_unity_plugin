namespace Fsm.State.Transition
{
    public class AlwaysValidTransition : FsmTransition
    {
        public AlwaysValidTransition(FsmStateBase nextState)
            : base(typeof(AlwaysValidTransition).Name, nextState, true) { }
    }
}
