using Fsm.State.Transition;
using System.Collections.Generic;

namespace Fsm.State
{
    public class FsmStateBase : FsmState
    {
        private string _stateName;
        private List<FsmTransition> _transitions;

        public string StateName => _stateName;
        public List<FsmTransition> Transitions => _transitions;

        public FsmStateBase(string stateName)
        {
            _stateName = stateName;
            _transitions = new List<FsmTransition>();
        }

        public void AddTransition(FsmTransition transition)
        {
            if (HasTransition(transition))
                return;

            Transitions.Add(transition);
        }

        private bool HasTransition(FsmTransition transition)
        {
            bool hasTransition = false;
            int transitionCount = Transitions.Count;

            for (int i = 0; i < transitionCount && !hasTransition; i++)
            {
                hasTransition = _transitions[i].TransitionName == transition.TransitionName
                    || _transitions[i].NextState.IsEquals(transition.NextState);
            }

            return hasTransition;
        }

        public virtual void Enter() {}

        public virtual void Exit() {}

        public virtual FsmState Update()
        {
            if (TryGetValidTransition(out FsmTransition validTransition))
            {
                return validTransition.NextState;
            }
            return this;
        }

        private bool TryGetValidTransition(out FsmTransition transition)
        {
            transition = null;
            int transitionCount = Transitions.Count;
            for (int i = 0; i < transitionCount && transition == null; i++)
            {
                if (Transitions[i].IsValid())
                {
                    transition = Transitions[i];
                }
            }
            return transition != null;
        }

        public bool IsEquals(FsmState state)
        {
            if (state == null)
            {
                return false;
            }

            return state.StateName == StateName;
        }

    }
}
