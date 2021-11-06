using UnityEngine;

namespace Fsm.State.Transition
{
    /// <summary>
    /// This interface is used to determine if a transition
    /// can be done.
    /// </summary>
    public abstract class FsmTransition : ScriptableObject
    {
        public abstract bool IsValid();
        public abstract string TransitionName { get; }
        public abstract FsmStateBase NextState { get; } 
    }
}