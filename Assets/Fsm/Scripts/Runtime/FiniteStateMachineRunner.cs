using Fsm.Core;
using System;
using UnityEngine;

namespace Fsm.RunTime
{
    public class FiniteStateMachineRunner : MonoBehaviour
    {
        [SerializeField]
        private FiniteStateMachine _finiteStateMachine;

        private bool _started;

        private void Start()
        {

        }

        private void Update()
        {
            if (!_started)
            {
                return;
            }

            try
            {
                _finiteStateMachine.Update();
            }
            catch (Exception e)
            {
                _started = false;
                Debug.LogException(e);
            }
        }

        public void Run()
        {
            _started = true;
        }

        public void Stop()
        {
            _started = false;
        }
    }
}
