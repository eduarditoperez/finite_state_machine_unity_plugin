using Fsm.Core;
using System;
using UnityEngine;

namespace Fsm.RunTime
{
    public class FiniteStateMachineRunner : MonoBehaviour
    {
        public FiniteStateMachine FiniteStateMachine => _finiteStateMachine;

        [SerializeField]
        private FiniteStateMachine _finiteStateMachine;

        [SerializeField]
        private bool _started;

        private void Start()
        {
            _finiteStateMachine = _finiteStateMachine.Clone();
            StartFiniteStateMachine();
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

        public void StartFiniteStateMachine()
        {
            _finiteStateMachine.Start();
            _started = true;
        }

        public void StopFiniteStateMachine()
        {
            _started = false;
            _finiteStateMachine.Stop();
        }
    }
}
