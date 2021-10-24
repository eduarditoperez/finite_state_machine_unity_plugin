using System.Collections.Generic;
using Fsm.State;
using Fsm.State.Transition;

internal class CounterFsmState : FsmState
{
    private int _counter;
    private string _stateName;
    private List<FsmTransition> _transitions;

    public int Counter => _counter;
    public string StateName => _stateName;
    public List<FsmTransition> Transitions => _transitions;

    public CounterFsmState(string stateName)
    {
        this._stateName = stateName;
    }

    public void AddTransition(FsmTransition transition)
    {
        _transitions.Add(transition);
    }

    public void Enter() {}
    public void Exit() {}

    public bool IsEquals(FsmState state)
    {
        return _stateName == state.StateName;
    }

    public FsmState Update()
    {
        try
        {
            _counter++;
        }
        catch (System.OverflowException)
        {
            _counter = 0;
        }
        return this;
    }

    // TODO: add a test that changes the active state,
    // for this we need a fsm with a valid transition to 
    // another state
}