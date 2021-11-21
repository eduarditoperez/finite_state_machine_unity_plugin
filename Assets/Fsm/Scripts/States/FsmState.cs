using Fsm.State.Transition;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    [CreateAssetMenu(fileName = "FsmState", menuName = "Fsm/State", order = 20)]
    public class FsmState : ScriptableObject
    {
        public string StateName;
        public List<FsmTransition> Transitions;
        public string Guid;
        public Vector2 Position;

        public void AddTransition(FsmTransition transition)
        {
            if (Transitions == null)
            {
                Transitions = new List<FsmTransition>();
            }

            if (HasTransition(transition))
            {
                return;
            }

            Transitions.Add(transition);
        }

        private bool HasTransition(FsmTransition transition)
        {
            bool hasTransition = false;

            if (Transitions == null)
            {
                return hasTransition;
            }

            int transitionCount = Transitions.Count;

            for (int i = 0; i < transitionCount && !hasTransition; i++)
            {
                hasTransition = Transitions[i].TransitionName == transition.TransitionName
                    || Transitions[i].NextState.IsEquals(transition.NextState);
            }

            return hasTransition;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual FsmState Clone()
        {
            FsmState clone = ScriptableObject.Instantiate(this);
            
            // Clone all transitions
            clone.Transitions = new List<FsmTransition>();
            this.Transitions.ForEach(transition => clone.AddTransition(transition.Clone()));
            return clone;
        }

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
                if (Transitions[i].IsValid)
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

        // TODO: not tested
        public FsmTransition GetTransitionToState(FsmState state)
        {
            FsmTransition fsmTransition = null;
            foreach (var transition in Transitions)
            {
                if (fsmTransition == null && transition.NextState.Equals(state))
                {
                    fsmTransition = transition;
                }
            }
            return fsmTransition;
        }

        // TODO: not tested
        public void RemoveTransition(FsmTransition transition)
        {
            if (transition == null)
            {
                return;
            }
            Transitions.Remove(transition);
        }

        // TODO: not tested
        public bool HasTransitions()
        {
            return Transitions != null && Transitions.Count > 0;
        }
    }
}
