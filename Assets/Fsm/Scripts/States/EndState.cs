using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    public class EndState : State
    {
        public override State Update()
        {
            return this;
        }
    }
}
