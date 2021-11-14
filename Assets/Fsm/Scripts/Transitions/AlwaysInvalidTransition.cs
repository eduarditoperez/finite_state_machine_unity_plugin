namespace Fsm.State.Transition
{
    public class AlwaysInvalidTransition : FsmTransition
    {
        public void Init(FsmState nextState)
        {
            TransitionName = typeof(AlwaysInvalidTransition).Name;
            NextState = nextState;
            IsValid = false;
        }
    }
}