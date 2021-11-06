namespace Fsm.State.Transition
{
    public class AlwaysInvalidTransition : FsmTransition
    {
        public void Init(FsmStateBase nextState)
        {
            TransitionName = typeof(AlwaysInvalidTransition).Name;
            NextState = nextState;
            IsValid = false;
        }
    }
}