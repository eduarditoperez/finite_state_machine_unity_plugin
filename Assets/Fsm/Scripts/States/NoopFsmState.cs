﻿namespace Fsm.State
{
    /// <summary>
    /// Placeholder state
    /// </summary>
    public class NoopFsmState : FsmState
    {
        public int EnterCounter { get; private set; }
        public int ExitCounter { get; private set; }

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
            return this;
        }
    }
}