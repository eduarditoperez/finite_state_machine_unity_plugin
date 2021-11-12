using Fsm.State;
using Fsm.Repository;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Fsm.State.Transition;

namespace Fsm.Core
{
    /// <summary>
    /// The FiniteStateMachine class contains a list of all states in 
    /// our FSM, as well as the initial state and the current active state.
    /// It also contains the central Update() function,
    /// which is called each tick and is responsible for running our 
    /// behavioral algorithm as follows:
    ///     - Call IsValid() on each transition in activeState.transtitions until
    ///       returns true or there are no more transitions.
    ///     - If a valid transition is found, then:
    ///         - Call activeState.Exit()
    ///         - Set activeState to validTransition.NextState
    ///         - Call activeState.Enter()
    ///     - If a valid transition is not found, then call _activeState.Update()
    /// </summary>
    [CreateAssetMenu(fileName = "FiniteStateMachine", menuName = "Fsm/Finite State Machine", order = 10)]
    public class FiniteStateMachine : ScriptableObject
    {
        public List<FsmStateBase> States;
        public FsmStateBase ActiveState;
        public FsmStateBase InitialState;

        public IAssetRepository AssetRepository { get; set; }

        // TODO: deprecate this function
        public void Init(FsmStateBase initialState)
        {
            InitialState = initialState;
            States = new List<FsmStateBase>();
        }

        public bool TryAddState(FsmStateBase state)
        {
            if (HasState(state))
            {
                return false;
            }

            if (States == null)
            {
                States = new List<FsmStateBase>();
            }

            States.Add(state);
            return true;
        }

        // TODO: not tested
        private bool HasState(FsmStateBase state)
        {
            if (States == null)
            {
                return false;
            }

            if (States.Count == 0)
            {
                return false;
            }

            bool hasTheState = false;
            for (int i = 0; i < States.Count && !hasTheState; i++)
            {
                if (States[i].StateName == state.StateName)
                {
                    hasTheState = true;
                }
            }
            return hasTheState;
        }

        // TODO: replace for TryRemove
        public void RemoveState(FsmStateBase state)
        {
            if (HasState(state))
            {
                int stateIndex = 0;
                bool stateFound = false;
                for (int i = 0; i < States.Count && !stateFound; i++)
                {
                    if (States[i].StateName == state.StateName)
                    {
                        stateFound = true;
                        stateIndex = i;
                    }
                }

                if (stateFound)
                {
                    FsmStateBase stateToRemove = States[stateIndex];
                    States.RemoveAt(stateIndex);
                    AssetRepository.RemoveObjectFromAsset(stateToRemove);
                }
            }
        }

        public void Start()
        {
            ActiveState = InitialState;
        }

        public void Stop()
        {
            ActiveState = null;
        }

        public void Update()
        {
            if (ActiveState != null)
            {
                ActiveState = ActiveState.Update();
            }
        }

        // TODO: not tested
        public bool TryCreateState(System.Type stateType, out FsmStateBase state)
        {
            state = ScriptableObject.CreateInstance(stateType) as FsmStateBase;
            state.name = stateType.Name;
            state.Guid = GUID.Generate().ToString();
            state.StateName = stateType.ToString();

            if (TryAddState(state))
            {
                AssetRepository.AddObjectToAsset(state, this);
                return true;
            }

            ScriptableObject.DestroyImmediate(state);
            state = null;
            return false;
        }

        // TODO: not tested
        public bool IsEmpty => !IsNotEmpty;
        // TODO: not tested
        public bool IsNotEmpty => States != null && States.Count > 0;

        public bool TryAddTransition(FsmStateBase fromState, FsmStateBase toState)
        {
            if (IsEmpty)
            {
                return false;
            }

            if (!HasState(fromState))
            {
                return false;
            }

            if (!HasState(toState))
            {
                return false;
            }

            if (HasTransition(fromState, toState))
            {
                return false;
            }

            FsmTransition transition = CreateTransition(fromState, toState);

            fromState.AddTransition(transition);

            return true;
        }

        private FsmTransition CreateTransition(FsmStateBase fromState, FsmStateBase toState)
        {
            string transitionName = $"{nameof(fromState)}_{nameof(toState)}";
            FsmTransition transition = ScriptableObject.CreateInstance<FsmTransition>();
            transition.name = transitionName;
            transition.NextState = toState;
            transition.TransitionName = transitionName;
            return transition;
        }

        public bool TryRemoveTransition(FsmStateBase fromState, FsmStateBase toState)
        {
            // Same as add but for removal
            return false;
        }

        public List<FsmStateBase> StatesToTransitionTo(FsmStateBase fromState)
        {
            // This function will give me the states we can transition to
            // from the fromState
            return null;
        }

        public bool HasTransition(FsmStateBase fromState, FsmStateBase toState)
        {
            bool transitionExists = false;
            if (HasState(fromState) && HasState(toState))
            {
                foreach (var transition in fromState.Transitions)
                {
                    if (transition.NextState.Equals(toState))
                    {
                        transitionExists = true;
                    }
                }
            }
            return transitionExists;
        }
    }
}
