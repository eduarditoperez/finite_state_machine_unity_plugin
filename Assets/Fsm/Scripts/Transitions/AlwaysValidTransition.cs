namespace Fsm.State.Transition
{
    public class AlwaysValidTransition : FsmTransition
    {
        public void Init(FsmStateBase nextState)
        {
            TransitionName = typeof(AlwaysValidTransition).Name;
            NextState = nextState;
            IsValid = true;
        }
    }
}
