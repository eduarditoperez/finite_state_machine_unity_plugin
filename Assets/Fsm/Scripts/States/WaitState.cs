using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    public class WaitState : FsmState
    {
        public float Seconds;
        private float _elapsedSeconds;

        public override void Enter()
        {
            base.Enter();
            ResetElapsedSeconds();
        }

        private void ResetElapsedSeconds()
        {
            _elapsedSeconds = 0;
        }

        public override void Exit()
        {
            base.Exit();
            ResetElapsedSeconds();
        }

        public override FsmState Update()
        {
            _elapsedSeconds += Time.deltaTime;
            if (_elapsedSeconds < Seconds)
            {
                return this;
            }

            _elapsedSeconds = 0;
            return base.Update();
        }
    }
}
