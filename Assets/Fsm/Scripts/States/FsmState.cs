using Fsm.State.Transition;
using System.Collections.Generic;

namespace Fsm.State
{
    public interface FsmState
    {
        string StateName { get; }
        List<FsmTransition> Transitions { get; }

        FsmState Update();
        void Enter();
        void Exit();
        void AddTransition(FsmTransition transition);
        bool IsEquals(FsmState state);
    }
}
