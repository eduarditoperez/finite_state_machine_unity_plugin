using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    public class EndState : FsmState
    {
        public override FsmState Update()
        {
            return this;
        }
    }
}
