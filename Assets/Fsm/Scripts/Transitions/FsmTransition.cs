namespace Fsm.State.Transition
{
    /// <summary>
    /// This interface is used to determine if a transition
    /// can be done.
    /// </summary>
    public interface FsmTransition
    {
        bool IsValid();
        string TransitionName { get; }
        FsmState NextState { get; }
    }
}