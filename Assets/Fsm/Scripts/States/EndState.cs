using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    public class EndState : FsmStateBase
    {
        public override FsmStateBase Update()
        {
            return this;
        }
    }
}
