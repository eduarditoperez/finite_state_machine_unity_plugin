using UnityEngine;

namespace Fsm.State.Transition
{
    /// <summary>
    /// This interface is used to determine if a transition
    /// can be done.
    /// </summary>
    [CreateAssetMenu(fileName = "Transition", menuName = "Fsm/State Transition", order = 30)]
    public class FsmTransition : ScriptableObject
    {
        [TextArea]
        public string description;

        public string TransitionName;
        public FsmState NextState;
        public bool IsValid;
        public string Guid;

        // TODO: deprecate this
        public virtual void Init(string transitionName, FsmState nextState, bool isValid)
        {
            TransitionName = transitionName;
            NextState = nextState;
            IsValid = isValid;
        }

        public virtual FsmTransition Clone()
        {
            return ScriptableObject.Instantiate(this);
        }
    }
}