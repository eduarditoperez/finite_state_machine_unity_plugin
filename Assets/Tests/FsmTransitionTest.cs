using Fsm.State.Transition;
using NUnit.Framework;
using UnityEngine;

public class FsmTransitionTest
{
    [Test]
    public void Clone_Creates_A_Clone_Of_The_Transition()
    {
        FsmTransition transition = GivenATransition();

        FsmTransition clone = transition.Clone();

        Assert.IsNotNull(clone);
        Assert.True(clone.name.Contains("Clone"));
        Assert.IsTrue(clone.name.Contains(transition.name));
        Assert.AreEqual(transition.Guid, clone.Guid);
        Assert.AreEqual(transition.TransitionName, clone.TransitionName);
        Assert.AreEqual(transition.TransitionType, clone.TransitionType);

    }

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
