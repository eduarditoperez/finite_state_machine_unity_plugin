using System;
using System.Collections.Generic;

namespace Fsm.State.Transition
{
    public static class ValidatorStrategyProvider
    {
        private static Dictionary<FsmTransitionType, ValidatorStrategy> _strategies;

        public static void Init()
        {
            if (_strategies == null)
            {
                _strategies = new Dictionary<FsmTransitionType, ValidatorStrategy>();
                foreach (FsmTransitionType transitionType in Enum.GetValues(typeof(FsmTransitionType)))
                {
                    _strategies.Add(transitionType, CreateValidatorStrategy(transitionType));
                }
            }
        }

        private static ValidatorStrategy CreateValidatorStrategy(FsmTransitionType transitionType)
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

        public static ValidatorStrategy ProvideStrategy(FsmTransitionType fsmTransitionType)
        {
            if (_strategies == null)
            {
                Init();
            }

            _strategies.TryGetValue(fsmTransitionType, out ValidatorStrategy validatorStrategy);
            return validatorStrategy;
        }
    }
}
