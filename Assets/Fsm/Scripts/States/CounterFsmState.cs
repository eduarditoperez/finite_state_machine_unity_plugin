using System.Collections.Generic;
using Fsm.State.Transition;

namespace Fsm.State
{
    public class CounterFsmState : FsmStateBase
    {
        private int _counter;
        private string _stateName;
        private List<FsmTransition> _transitions;

        public int Counter => _counter;

        //public CounterFsmState(string stateName) : base(stateName) {}

        public override FsmStateBase Update()
        {
            try
            {
                _counter++;
            }
            catch (System.OverflowException)
            {
                _counter = 0;
            }
            return base.Update();
        }
    }
}