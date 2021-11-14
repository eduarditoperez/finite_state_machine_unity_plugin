using UnityEngine;

namespace Fsm.State.Transition
{
    /// <summary>
    /// This script represetns a transition between states inside a FSM.
    /// </summary>
    [CreateAssetMenu(fileName = "Transition", menuName = "Fsm/State Transition", order = 30)]
    public class FsmTransition : ScriptableObject
    {
        [TextArea]
        public string description;

        public FsmTransitionType TransitionType;
        public string TransitionName;
        public FsmState NextState;
        public bool IsValid;
        public string Guid;

        public virtual FsmTransition Clone()
        {
            return ScriptableObject.Instantiate(this);
        }
    }
}