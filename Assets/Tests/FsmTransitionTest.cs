using Fsm.State.Transition;
using NUnit.Framework;
using UnityEngine;

namespace Fsm.State.Transition.Test
{
    public class FsmTransitionTest
    {
        private static FsmTransition GivenATransition()
        {
            FsmTransition transition = ScriptableObject.CreateInstance<FsmTransition>();
            transition.name = "aTransition";
            transition.TransitionName = "TransitionName";
            transition.Guid = "12345";
            transition.TransitionType = FsmTransitionType.ValidTransition;
            return transition;
        }
    }
}
