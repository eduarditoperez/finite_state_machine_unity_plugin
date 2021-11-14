using Fsm.State;
using Fsm.Repository;
using System.Collections.Generic;
using System.Linq;
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
        public List<Fsm.State.State> States;
        public Fsm.State.State ActiveState;
        public Fsm.State.State InitialState;

        private Fsm.State.State _currentActiveState;

        public IAssetRepository AssetRepository { get; set; }

        // TODO: deprecate this function
        public void Init(Fsm.State.State initialState)
        {
            InitialState = initialState;
            States = new List<Fsm.State.State>();
        }

        public bool TryAddState(Fsm.State.State state)
        {
            if (HasState(state))
            {
                return false;
            }

            if (States == null)
            {
                States = new List<Fsm.State.State>();
            }

            // TODO: not tested
            if (state is RootState)
            {
                InitialState = state;
            }

            States.Add(state);
            return true;
        }

        // TODO: not tested
        private bool HasState(Fsm.State.State state)
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
        public void RemoveState(Fsm.State.State state)
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
                    Fsm.State.State stateToRemove = States[stateIndex];
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
                _currentActiveState = ActiveState.Update();
                if (!_currentActiveState.Equals(ActiveState))
                {
                    ActiveState.Exit();
                    _currentActiveState.Enter();
                }

                ActiveState = _currentActiveState;
            }
        }

        // TODO: not tested
        public bool TryCreateState(System.Type stateType, out State.State state)
        {
            state = ScriptableObject.CreateInstance(stateType) as State.State;
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

        public bool TryAddTransition(State.State fromState, State.State toState)
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

            FsmTransition transition = CreateTransition(typeof(FsmTransition), fromState, toState);
            fromState.AddTransition(transition);

            AssetRepository.AddObjectToAsset(transition, this);

            return true;
        }

        public bool HasTransition(State.State fromState, State.State toState)
        {
            bool transitionExists = false;
            if (HasState(fromState) && HasState(toState))
            {
                if (fromState.HasTransitions())
                {
                    foreach (var transition in fromState.Transitions)
                    {
                        if (transition.NextState.Equals(toState))
                        {
                            transitionExists = true;
                        }
                    }
                }
            }
            return transitionExists;
        }

        private FsmTransition CreateTransition(System.Type transitionType, State.State fromState, State.State toState)
        {
            string transitionName = $"{fromState.GetType().Name}_to_{toState.GetType().Name}";
            FsmTransition transition = ScriptableObject.CreateInstance(transitionType) as FsmTransition;
            transition.name = transitionName;
            transition.NextState = toState;
            transition.TransitionName = transitionName;
            transition.Guid = GUID.Generate().ToString();

            return transition;
        }

        public bool TryRemoveTransition(State.State fromState, State.State toState)
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
                FsmTransition transition = fromState.GetTransitionToState(toState);
                fromState.RemoveTransition(transition);

                AssetRepository.RemoveObjectFromAsset(transition);

                return true;
            }

            return false;
        }

        public List<State.State> GetReachableStates(State.State state)
        {
            return state.Transitions.Select(transition => transition.NextState).ToList();
        }

        internal FiniteStateMachine Clone()
        {
            FiniteStateMachine clone = ScriptableObject.Instantiate(this);
            States.ForEach(state => clone.TryAddState(state.Clone()));

            return clone;
        }
    }
}
