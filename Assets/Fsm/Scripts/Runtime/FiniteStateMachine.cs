using Fsm.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

                    // TODO: add an interface to deal with this
                    AssetDatabase.RemoveObjectFromAsset(stateToRemove);
                    AssetDatabase.SaveAssets();
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
                // TODO: add an interface for this
                AssetDatabase.AddObjectToAsset(state, this);
                AssetDatabase.SaveAssets();
                return true;
            }

            ScriptableObject.DestroyImmediate(state);
            state = null;
            return false;
        }

        // TODO: not tested
        public bool HasStates => States != null && States.Count > 0;
    }
}
