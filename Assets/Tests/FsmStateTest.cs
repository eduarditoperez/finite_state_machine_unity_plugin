using NUnit.Framework;
using Fsm.State.Transition;
using UnityEngine;
using System.Collections.Generic;

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
                FsmState startState = GivenAnFsmState(stateName);
                // Then
                Assert.AreEqual(stateName, startState.StateName);
            }

            [Test]
            public void Constructor_Creates_State_With_Empty_Transitions()
            {
                FsmState startState = GivenAnFsmState(string.Empty);
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

                FsmState startState = GivenAnFsmState(string.Empty);
                startState.AddTransition(transition);

                Assert.IsNotEmpty(startState.Transitions);
                Assert.AreEqual(1, startState.Transitions.Count);
            }

            [Test]
            public void AddTransition_Adding_The_Same_Transition_Twice_Does_Nothing()
            {
                FsmState fsmState = GivenANoopState(StateName1);
                FsmTransition transition = GivenANoopTransition(TransitionName1, fsmState);

                FsmState startState = GivenAnFsmState(string.Empty);

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

                FsmState startState = GivenAnFsmState(string.Empty);

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

                FsmState startState = GivenAnFsmState(string.Empty);

                startState.AddTransition(transition1);
                startState.AddTransition(transition2);

                Assert.IsNotEmpty(startState.Transitions);
                Assert.AreEqual(1, startState.Transitions.Count);
            }

            [Test]
            public void AddTransition_Adding_Transition_To_Self_Adds_The_Transition()
            {
                FsmState startState = GivenAnFsmState("baseState");

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
                FsmState startState = GivenAnFsmState("startState");
                FsmTransition invalidTransition = GivenAnAlwaysInvalidTransition(startState);
                startState.AddTransition(invalidTransition);

                FsmState nextState = startState.Update();

                Assert.AreEqual(startState, nextState);
            }

            [Test]
            public void Update_Returns_Next_State_When_Transition_Is_Valid()
            {
                FsmState expectedState = GivenAnFsmState("expectedState");
                FsmTransition validTransition = GivenAnAlwaysValidTransition(expectedState);

                FsmState startState = GivenAnFsmState("startState");
                startState.AddTransition(validTransition);

                FsmState nextState = startState.Update();

                Assert.AreEqual(expectedState, nextState);
            }

            [Test]
            public void Update_Returns_The_State_With_Valid_Transition()
            {
                FsmState expectedState = GivenAnFsmState("expectedState");

                FsmState startState = GivenAnFsmState("startState");
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
                FsmState stateA = GivenAnFsmState("stateA");

                stateA.AddTransition(GivenAnAlwaysValidTransition(stateA));

                Assert.True(stateA.IsEquals(stateA));
            }

            [Test]
            public void IsEquals_Returns_False_When_Comparing_Different_States()
            {
                FsmState stateA = GivenAnFsmState("stateA");
                FsmState stateB = GivenAnFsmState("stateB");

                stateA.AddTransition(GivenAnAlwaysValidTransition(stateB));
                stateB.AddTransition(GivenAnAlwaysValidTransition(stateA));

                Assert.False(stateA.IsEquals(stateB));
            }

            [Test]
            public void IsEquals_Returns_True_For_Two_Identical_States()
            {
                FsmState stateA = GivenAnFsmState("stateA");
                FsmState stateB = GivenAnFsmState("stateA");

                stateA.AddTransition(GivenAnAlwaysValidTransition(stateA));
                stateB.AddTransition(GivenAnAlwaysValidTransition(stateB));

                Assert.True(stateA.IsEquals(stateB));
            }
        }

        public class CloneTests
        {
            [Test]
            public void Clone_Creates_A_Clone()
            {
                FsmState fsmState = GivenAnFsmState("state");
                fsmState.AddTransition(GivenAnAlwaysValidTransition(fsmState));

                FsmState clone = fsmState.Clone();

                Assert.IsNotNull(clone);
                Assert.IsTrue(clone.name.Contains("Clone"));

                foreach (FsmTransition t in clone.Transitions)
                {
                    Assert.IsTrue(t.name.Contains("Clone"));
                }
            }
        }

        private static FsmState GivenAnFsmState(string stateName)
        {
            FsmState fsmState = ScriptableObject.CreateInstance<FsmState>();
            fsmState.StateName = stateName;
            fsmState.Transitions = new List<FsmTransition>();
            fsmState.Guid = "1234";
            fsmState.Position = Vector2.zero;
            return fsmState;
        }

        private static NoopTransition GivenANoopTransition(string transitionName, FsmState fsmState)
        {
            NoopTransition noopTransition = ScriptableObject.CreateInstance<NoopTransition>();
            noopTransition.TransitionName = transitionName;
            noopTransition.NextState = fsmState;
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
            transition.TransitionName = typeof(AlwaysValidTransition).Name;
            transition.NextState = state;
            transition.TransitionType = FsmTransitionType.ValidTransition;
            return transition;
        }

        private static FsmTransition GivenAnAlwaysInvalidTransition(FsmState state)
        {
            AlwaysInvalidTransition transition = ScriptableObject.CreateInstance<AlwaysInvalidTransition>();
            transition.TransitionName = typeof(AlwaysInvalidTransition).Name;
            transition.NextState = state;
            transition.TransitionType = FsmTransitionType.InvalidTransition;
            return transition;
        }
    }
}
