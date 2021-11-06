using NUnit.Framework;
using Fsm.State.Transition;
using Fsm.State;
using UnityEngine;

public class FsmStateBaseTest
{
    private const string TransitionName1 = "Transition1";
    private const string TransitionName2 = "Transition2";

    private const string StateName1 = "StateName1";
    private const string StateName2 = "StateName2";

    public class FsmConstructorTest
    {
        [Test]
        public void Constructor_Creates_State_With_The_Given_Name()
        {
            // Given
            string stateName = "startState";
            // When
            FsmStateBase startState = GivenAnFsmStateBase(stateName);
            // Then
            Assert.AreEqual(stateName, startState.StateName);
        }

        [Test]
        public void Constructor_Creates_State_With_Empty_Transitions()
        {
            FsmStateBase startState = GivenAnFsmStateBase(string.Empty);
            Assert.AreEqual(0, startState.Transitions.Count);
        }
    }

    public class AddTransitionTests
    {
        [Test]
        public void AddTransition_Adds_The_Given_Transition()
        {
            FsmStateBase fsmState = GivenANoopState(StateName1);
            FsmTransition transition = GivenANoopTransition(TransitionName1, fsmState);

            FsmStateBase startState = GivenAnFsmStateBase(string.Empty);
            startState.AddTransition(transition);

            Assert.IsNotEmpty(startState.Transitions);
            Assert.AreEqual(1, startState.Transitions.Count);
        }

        [Test]
        public void AddTransition_Adding_The_Same_Transition_Twice_Does_Nothing()
        {
            FsmStateBase fsmState = GivenANoopState(StateName1);
            FsmTransition transition = GivenANoopTransition(TransitionName1, fsmState);

            FsmStateBase startState = GivenAnFsmStateBase(string.Empty);

            startState.AddTransition(transition);
            startState.AddTransition(transition);

            Assert.IsNotEmpty(startState.Transitions);
            Assert.AreEqual(1, startState.Transitions.Count);
        }

        [Test]
        public void AddTransition_Adding_Two_Transitions_With_DifferentNames_Adds_The_Transitions()
        {
            FsmStateBase fsmState1 = GivenANoopState(StateName1);
            FsmStateBase fsmState2 = GivenANoopState(StateName2);

            FsmTransition transition1 = GivenANoopTransition(TransitionName1, fsmState1);
            FsmTransition transition2 = GivenANoopTransition(TransitionName2, fsmState2);

            FsmStateBase startState = GivenAnFsmStateBase(string.Empty);

            startState.AddTransition(transition1);
            startState.AddTransition(transition2);

            Assert.IsNotEmpty(startState.Transitions);
            Assert.AreEqual(2, startState.Transitions.Count);
        }

        [Test]
        public void AddTransition_Adding_Two_Transitions_To_The_Same_State_Does_Nothign()
        {
            FsmStateBase fsmState1 = GivenANoopState(StateName1);

            FsmTransition transition1 = GivenANoopTransition(TransitionName1, fsmState1);
            FsmTransition transition2 = GivenANoopTransition(TransitionName2, fsmState1);

            FsmStateBase startState = GivenAnFsmStateBase(string.Empty);

            startState.AddTransition(transition1);
            startState.AddTransition(transition2);

            Assert.IsNotEmpty(startState.Transitions);
            Assert.AreEqual(1, startState.Transitions.Count);
        }

        [Test]
        public void AddTransition_Adding_Transition_To_Self_Adds_The_Transition()
        {
            FsmStateBase startState = GivenAnFsmStateBase("baseState");
  
            FsmTransition transition1 = GivenANoopTransition(TransitionName1, startState);

            startState.AddTransition(transition1);

            FsmTransition transition = startState.Transitions[0];
            Assert.AreEqual(startState, transition.NextState);
        }

    }

    public class UpdateTests
    {
        [Test]
        public void Update_Returs_Self_If_There_Is_No_Valid_Transition()
        {
            FsmStateBase startState = GivenAnFsmStateBase("startState");
            FsmTransition invalidTransition = GivenAnAlwaysInvalidTransition(startState);
            startState.AddTransition(invalidTransition);

            FsmStateBase nextState = startState.Update();

            Assert.AreEqual(startState, nextState);
        }

        [Test]
        public void Update_Returns_Next_State_When_Transition_Is_Valid()
        {
            FsmStateBase expectedState = GivenAnFsmStateBase("expectedState");
            FsmTransition validTransition = GivenAnAlwaysValidTransition(expectedState);

            FsmStateBase startState = GivenAnFsmStateBase("startState");
            startState.AddTransition(validTransition);

            FsmStateBase nextState = startState.Update();

            Assert.AreEqual(expectedState, nextState);
        }

        [Test]
        public void Update_Returns_The_State_With_Valid_Transition()
        {
            FsmStateBase expectedState = GivenAnFsmStateBase("expectedState");

            FsmStateBase startState = GivenAnFsmStateBase("startState");
            startState.AddTransition(GivenAnAlwaysInvalidTransition(startState));
            startState.AddTransition(GivenAnAlwaysValidTransition(expectedState));

            FsmStateBase nextState = startState.Update();

            Assert.AreEqual(expectedState, nextState);
        }

    }

    public class IsEqualsTests
    {
        [Test]
        public void IsEquals_Returns_True_When_Comparing_The_Same_State()
        {
            FsmStateBase stateA = GivenAnFsmStateBase("stateA");

            stateA.AddTransition(GivenAnAlwaysValidTransition(stateA));

            Assert.True(stateA.IsEquals(stateA));
        }

        [Test]
        public void IsEquals_Returns_False_When_Comparing_Different_States()
        {
            FsmStateBase stateA = GivenAnFsmStateBase("stateA");
            FsmStateBase stateB = GivenAnFsmStateBase("stateB");

            stateA.AddTransition(GivenAnAlwaysValidTransition(stateB));
            stateB.AddTransition(GivenAnAlwaysValidTransition(stateA));

            Assert.False(stateA.IsEquals(stateB));
        }

        [Test]
        public void IsEquals_Returns_True_For_Two_Identical_States()
        {
            FsmStateBase stateA = GivenAnFsmStateBase("stateA");
            FsmStateBase stateB = GivenAnFsmStateBase("stateA");

            stateA.AddTransition(GivenAnAlwaysValidTransition(stateA));
            stateB.AddTransition(GivenAnAlwaysValidTransition(stateB));

            Assert.True(stateA.IsEquals(stateB));
        }
    }

    public class InitTests
    {
        [Test]
        public void Init_Set_StateName_To_EmptyString()
        {
            FsmStateBase fsmStateBase = GivenAnFsmStateBase(string.Empty);

            fsmStateBase.Init(string.Empty);

            Assert.True(string.IsNullOrEmpty(fsmStateBase.StateName));

        }

        [Test]
        public void Init_Set_StateName_To_The_Name_Used_As_Argument()
        {
            const string stateName = "This is the name";
            FsmStateBase fsmStateBase = GivenAnFsmStateBase(string.Empty);

            fsmStateBase.Init(stateName);

            Assert.AreEqual(stateName, fsmStateBase.StateName);
        }
    }

    private static FsmStateBase GivenAnFsmStateBase(string stateName)
    {
        FsmStateBase fsmStateBase = ScriptableObject.CreateInstance<FsmStateBase>();
        fsmStateBase.Init(stateName);
        return fsmStateBase;
    }

    private static NoopTransition GivenANoopTransition(string transitionName, FsmStateBase fsmState)
    {
        NoopTransition noopTransition = ScriptableObject.CreateInstance<NoopTransition>();

        noopTransition.Init(transitionName, fsmState);

        return noopTransition;
    }

    private static FsmStateBase GivenANoopState(string stateName)
    {
        NoopFsmState state = ScriptableObject.CreateInstance<NoopFsmState>();
        state.StateName = stateName;
        return state;
    }

    private static FsmTransition GivenAnAlwaysValidTransition(FsmStateBase state)
    {
        AlwaysValidTransition transition = ScriptableObject.CreateInstance<AlwaysValidTransition>();
        transition.Init(state);
        return transition;
    }

    private static FsmTransition GivenAnAlwaysInvalidTransition(FsmStateBase state)
    {
        AlwaysInvalidTransition transition = ScriptableObject.CreateInstance<AlwaysInvalidTransition>();
        transition.Init(state);
        return transition;
    }
}
