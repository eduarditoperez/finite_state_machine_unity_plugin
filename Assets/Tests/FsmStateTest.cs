using NUnit.Framework;
using Fsm.State.Transition;
using UnityEngine;

namespace Fsm.State.Test
{
    public class FsmStateTest
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
                FsmState startState = GivenAnFsmStateBase(stateName);
                // Then
                Assert.AreEqual(stateName, startState.StateName);
            }

            [Test]
            public void Constructor_Creates_State_With_Empty_Transitions()
            {
                FsmState startState = GivenAnFsmStateBase(string.Empty);
                Assert.AreEqual(0, startState.Transitions.Count);
            }
        }

        public class AddTransitionTests
        {
            [Test]
            public void AddTransition_Adds_The_Given_Transition()
            {
                FsmState fsmState = GivenANoopState(StateName1);
                FsmTransition transition = GivenANoopTransition(TransitionName1, fsmState);

                FsmState startState = GivenAnFsmStateBase(string.Empty);
                startState.AddTransition(transition);

                Assert.IsNotEmpty(startState.Transitions);
                Assert.AreEqual(1, startState.Transitions.Count);
            }

            [Test]
            public void AddTransition_Adding_The_Same_Transition_Twice_Does_Nothing()
            {
                FsmState fsmState = GivenANoopState(StateName1);
                FsmTransition transition = GivenANoopTransition(TransitionName1, fsmState);

                FsmState startState = GivenAnFsmStateBase(string.Empty);

                startState.AddTransition(transition);
                startState.AddTransition(transition);

                Assert.IsNotEmpty(startState.Transitions);
                Assert.AreEqual(1, startState.Transitions.Count);
            }

            [Test]
            public void AddTransition_Adding_Two_Transitions_With_DifferentNames_Adds_The_Transitions()
            {
                FsmState fsmState1 = GivenANoopState(StateName1);
                FsmState fsmState2 = GivenANoopState(StateName2);

                FsmTransition transition1 = GivenANoopTransition(TransitionName1, fsmState1);
                FsmTransition transition2 = GivenANoopTransition(TransitionName2, fsmState2);

                FsmState startState = GivenAnFsmStateBase(string.Empty);

                startState.AddTransition(transition1);
                startState.AddTransition(transition2);

                Assert.IsNotEmpty(startState.Transitions);
                Assert.AreEqual(2, startState.Transitions.Count);
            }

            [Test]
            public void AddTransition_Adding_Two_Transitions_To_The_Same_State_Does_Nothign()
            {
                FsmState fsmState1 = GivenANoopState(StateName1);

                FsmTransition transition1 = GivenANoopTransition(TransitionName1, fsmState1);
                FsmTransition transition2 = GivenANoopTransition(TransitionName2, fsmState1);

                FsmState startState = GivenAnFsmStateBase(string.Empty);

                startState.AddTransition(transition1);
                startState.AddTransition(transition2);

                Assert.IsNotEmpty(startState.Transitions);
                Assert.AreEqual(1, startState.Transitions.Count);
            }

            [Test]
            public void AddTransition_Adding_Transition_To_Self_Adds_The_Transition()
            {
                FsmState startState = GivenAnFsmStateBase("baseState");

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
                FsmState startState = GivenAnFsmStateBase("startState");
                FsmTransition invalidTransition = GivenAnAlwaysInvalidTransition(startState);
                startState.AddTransition(invalidTransition);

                FsmState nextState = startState.Update();

                Assert.AreEqual(startState, nextState);
            }

            [Test]
            public void Update_Returns_Next_State_When_Transition_Is_Valid()
            {
                FsmState expectedState = GivenAnFsmStateBase("expectedState");
                FsmTransition validTransition = GivenAnAlwaysValidTransition(expectedState);

                FsmState startState = GivenAnFsmStateBase("startState");
                startState.AddTransition(validTransition);

                FsmState nextState = startState.Update();

                Assert.AreEqual(expectedState, nextState);
            }

            [Test]
            public void Update_Returns_The_State_With_Valid_Transition()
            {
                FsmState expectedState = GivenAnFsmStateBase("expectedState");

                FsmState startState = GivenAnFsmStateBase("startState");
                startState.AddTransition(GivenAnAlwaysInvalidTransition(startState));
                startState.AddTransition(GivenAnAlwaysValidTransition(expectedState));

                FsmState nextState = startState.Update();

                Assert.AreEqual(expectedState, nextState);
            }

        }

        public class IsEqualsTests
        {
            [Test]
            public void IsEquals_Returns_True_When_Comparing_The_Same_State()
            {
                FsmState stateA = GivenAnFsmStateBase("stateA");

                stateA.AddTransition(GivenAnAlwaysValidTransition(stateA));

                Assert.True(stateA.IsEquals(stateA));
            }

            [Test]
            public void IsEquals_Returns_False_When_Comparing_Different_States()
            {
                FsmState stateA = GivenAnFsmStateBase("stateA");
                FsmState stateB = GivenAnFsmStateBase("stateB");

                stateA.AddTransition(GivenAnAlwaysValidTransition(stateB));
                stateB.AddTransition(GivenAnAlwaysValidTransition(stateA));

                Assert.False(stateA.IsEquals(stateB));
            }

            [Test]
            public void IsEquals_Returns_True_For_Two_Identical_States()
            {
                FsmState stateA = GivenAnFsmStateBase("stateA");
                FsmState stateB = GivenAnFsmStateBase("stateA");

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
                FsmState fsmStateBase = GivenAnFsmStateBase(string.Empty);

                fsmStateBase.Init(string.Empty);

                Assert.True(string.IsNullOrEmpty(fsmStateBase.StateName));

            }

            [Test]
            public void Init_Set_StateName_To_The_Name_Used_As_Argument()
            {
                const string stateName = "This is the name";
                FsmState fsmStateBase = GivenAnFsmStateBase(string.Empty);

                fsmStateBase.Init(stateName);

                Assert.AreEqual(stateName, fsmStateBase.StateName);
            }
        }

        private static FsmState GivenAnFsmStateBase(string stateName)
        {
            FsmState fsmStateBase = ScriptableObject.CreateInstance<FsmState>();
            fsmStateBase.Init(stateName);
            return fsmStateBase;
        }

        private static NoopTransition GivenANoopTransition(string transitionName, FsmState fsmState)
        {
            NoopTransition noopTransition = ScriptableObject.CreateInstance<NoopTransition>();

            noopTransition.Init(transitionName, fsmState);

            return noopTransition;
        }

        private static FsmState GivenANoopState(string stateName)
        {
            NoopFsmState state = ScriptableObject.CreateInstance<NoopFsmState>();
            state.StateName = stateName;
            return state;
        }

        private static FsmTransition GivenAnAlwaysValidTransition(FsmState state)
        {
            AlwaysValidTransition transition = ScriptableObject.CreateInstance<AlwaysValidTransition>();
            transition.Init(state);
            return transition;
        }

        private static FsmTransition GivenAnAlwaysInvalidTransition(FsmState state)
        {
            AlwaysInvalidTransition transition = ScriptableObject.CreateInstance<AlwaysInvalidTransition>();
            transition.Init(state);
            return transition;
        }
    }
}
