using UnityEngine;
using System;
using System.Threading.Tasks;

namespace Fsm.State
{
    public class DebugLogState : FsmState
    {
        public string Message;

        public override FsmState Update()
        {
            Debug.Log(Message);
            return base.Update();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
