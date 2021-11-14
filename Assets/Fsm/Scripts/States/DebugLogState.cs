using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    public class DebugLogState : State
    {
        public string Message;

        public override State Update()
        {
            Debug.Log(Message);
            return base.Update();
        }
    }
}
