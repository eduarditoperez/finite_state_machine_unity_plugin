namespace Fsm.State.Transition
{
    public class AlwaysValidTransition : FsmTransition
    {
        public void Init(State nextState)
        {
            TransitionName = typeof(AlwaysValidTransition).Name;
            NextState = nextState;
            IsValid = true;
        }
    }
}
