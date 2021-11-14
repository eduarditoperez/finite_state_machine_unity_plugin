using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
