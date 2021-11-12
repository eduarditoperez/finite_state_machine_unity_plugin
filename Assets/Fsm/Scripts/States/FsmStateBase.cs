using Fsm.State.Transition;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    [CreateAssetMenu(fileName = "FsmState", menuName = "Fsm/State", order = 20)]
    public class FsmStateBase : ScriptableObject
    {
        public string StateName;
        public List<FsmTransition> Transitions;
        public string Guid;
        public Vector2 Position;

        public void Init(string stateName)
        {
            StateName = stateName;
            Transitions = new List<FsmTransition>();
        }

        public void AddTransition(FsmTransition transition)
        {
            if (HasTransition(transition))
            {
                return;
            }

            Transitions.Add(transition);
        }

        private bool HasTransition(FsmTransition transition)
        {
            bool hasTransition = false;
            int transitionCount = Transitions.Count;

            for (int i = 0; i < transitionCount && !hasTransition; i++)
            {
                hasTransition = Transitions[i].TransitionName == transition.TransitionName
                    || Transitions[i].NextState.IsEquals(transition.NextState);
            }

            return hasTransition;
        }

        public virtual void Enter() {}
        public virtual void Exit() {}

        public virtual FsmStateBase Update()
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

        public bool IsEquals(FsmStateBase state)
        {
            if (state == null)
            {
                return false;
            }

            return state.StateName == StateName;
        }

        // TODO: not tested
        public FsmTransition GetTransitionToState(FsmStateBase state)
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
    }
}
