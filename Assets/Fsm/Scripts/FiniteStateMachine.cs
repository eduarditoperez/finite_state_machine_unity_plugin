using Fsm.State;
using System.Collections.Generic;

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
    [System.Serializable]
    public class FiniteStateMachine
    {
        public List<FsmState> States => _states;
        public FsmState ActiveState => _activeState;
        public FsmState InitialState => _initialState;

        private List<FsmState> _states;
        private FsmState _initialState;
        private FsmState _activeState;

        public FiniteStateMachine()
        {
            _states = new List<FsmState>();
        }

        public FiniteStateMachine(FsmState initialState) : this()
        {
            _initialState = initialState;
        }

        public void AddState(FsmState state)
        {
            if (HasState(state))
            {
                return;
            }
            _states.Add(state);
        }

        private bool HasState(FsmState state)
        {
            bool hasTheState = false;
            for (int i = 0; i < _states.Count && !hasTheState; i++)
            {
                if (_states[i].StateName == state.StateName)
                {
                    hasTheState = true;
                }
            }
            return hasTheState;
        }

        public void RemoveState(FsmState state)
        {
            if (HasState(state))
            {
                int stateIndex = 0;
                bool stateFound = false;
                for (int i = 0; i < _states.Count && !stateFound; i++)
                {
                    if (_states[i].StateName == state.StateName)
                    {
                        stateFound = true;
                        stateIndex = i;
                    }
                }
                if (stateFound)
                {
                    _states.RemoveAt(stateIndex);
                }
            }
        }

        public void Start()
        {
            _activeState = _initialState;
        }

        public void Stop()
        {
            _activeState = null;
        }

        public void Update()
        {
            if (_activeState != null)
            {
                _activeState = _activeState.Update();
            }
        }

    }
}
