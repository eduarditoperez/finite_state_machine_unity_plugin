using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsm.State
{
    public class WaitState : State
    {
        public float Seconds;
        private float _elapsedSeconds;

        public override void Enter()
        {
            ResetElapsedSeconds();
        }

        private void ResetElapsedSeconds()
        {
            _elapsedSeconds = 0;
        }

        public override void Exit()
        {
            ResetElapsedSeconds();
        }

        public override State Update()
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
