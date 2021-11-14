using System;
using System.Collections.Generic;

namespace Fsm.State.Transition
{
    public class ValidatorStrategyProvider
    {
        private Dictionary<FsmTransitionType, ValidatorStrategy> _strategies;

        public ValidatorStrategyProvider()
        {
            UnityEngine.Debug.Log($"ValidatorStrategyProvider");
            _strategies = new Dictionary<FsmTransitionType, ValidatorStrategy>();
            foreach (FsmTransitionType transitionType in Enum.GetValues(typeof(FsmTransitionType)))
            {
                UnityEngine.Debug.Log($"Adding transitionType {transitionType}");
                _strategies.Add(transitionType, CreateValidatorStrategy(transitionType));
            }
        }

        private ValidatorStrategy CreateValidatorStrategy(FsmTransitionType transitionType)
        {
            // TODO: more complex validators will need a context
            // to know how to initialize them
            switch (transitionType)
            {
                case FsmTransitionType.InvalidTransition:
                    return new AlwaysFalseValidator();
                case FsmTransitionType.ValidTransition:
                    return new AlwaysTrueValidator();
            };

            return new AlwaysTrueValidator();
        }

        public ValidatorStrategy ProvideStrategy(FsmTransitionType fsmTransitionType)
        {
            UnityEngine.Debug.Log($"ProvideStrategy for TransitionType {fsmTransitionType}");
            if (_strategies
                .TryGetValue(fsmTransitionType,
                out ValidatorStrategy validatorStrategy))
            {
                UnityEngine.Debug.Log("ValidatorStrategy not found");
                return validatorStrategy;
            }
            return null;
        }
    }
}
