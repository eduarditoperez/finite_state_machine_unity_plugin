using System.Collections.Generic;
using Fsm.State.Transition;

namespace Fsm.State
{
    public class CounterFsmState : FsmState
    {
        public int Counter { get; private set; }
        public int EnterCounter { get; private set; }
        public int ExitCounter { get; private set; }

        private string _stateName;
        private List<FsmTransition> _transitions;

        public override void Enter()
        {
            EnterCounter++;
        }

        public override void Exit()
        {
            ExitCounter++;
        }

        public override FsmState Update()
        {
            try
            {
                Counter++;
            }
            catch (System.OverflowException)
            {
                Counter = 0;
            }
            return base.Update();
        }
    }
}