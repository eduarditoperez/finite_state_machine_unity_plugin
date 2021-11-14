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
        public bool IsValid => IsValidTransition();
        public string Guid;

        private ValidatorStrategy _validatorStrategy;

        public virtual FsmTransition Clone()
        {
            return ScriptableObject.Instantiate(this);
        }

        private bool IsValidTransition()
        {
            ValidatorStrategyProvider.Init();
            _validatorStrategy = ValidatorStrategyProvider.ProvideStrategy(TransitionType);
            return _validatorStrategy.IsValid();
        }
    }
}