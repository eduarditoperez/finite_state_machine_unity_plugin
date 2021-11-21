using Fsm.Core;
using NUnit.Framework;
using Fsm.State;
using Fsm.State.Transition;
using UnityEngine;
using System;
using Fsm.Repository;
using System.Collections.Generic;
using Fsm.Utility;

public class FsmTest
{
    public class InitTests
    {
        [Test]
        public void Init_Creates_Empty_States()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);
            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void Init_Doesnt_Have_An_ActiveState()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();

            Assert.IsNull(fsm.ActiveState);
        }

        [Test]
        public void Init_Doesnt_Have_An_InitialState()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();

            Assert.IsNull(fsm.InitialState);
        }

        [Test]
        public void Init_With_Initial_State_Set_InitialState()
        {
            FsmState initialState = GivenANoopInitializedState("initialState");
            FiniteStateMachine fsm = GivenAFiniteStateMachine(initialState);

            Assert.IsEmpty(fsm.States);
            Assert.IsNotNull(fsm.InitialState);
            Assert.True(fsm.InitialState.IsEquals(initialState));
        }
    }

    public class AddStateTests
    {
        [Test]
        public void AddState_Adds_The_State()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");

            fsm.TryAddState(state);

            Assert.IsNotEmpty(fsm.States);
        }

        [Test]
        public void AddState_Doesnt_Add_The_State_Twice()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");

            fsm.TryAddState(state);
            fsm.TryAddState(state);

            Assert.AreEqual(1, fsm.States.Count);
        }

        [Test]
        public void AddState_Adds_Different_States()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState stateA = GivenANoopInitializedState("StateA");
            FsmState stateB = GivenANoopInitializedState("StateB");

            fsm.TryAddState(stateA);
            fsm.TryAddState(stateB);

            Assert.AreEqual(2, fsm.States.Count);
        }

        [Test]
        public void AddState_Returns_True_After_Adding_The_State()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");

            Assert.True(fsm.TryAddState(state));
        }

        [Test]
        public void AddState_Returns_False_When_We_Add_The_Same_State_Twice()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");

            fsm.TryAddState(state);
            Assert.False(fsm.TryAddState(state));
        }
    }

    public class RemoveStateTests
    {
        [Test]
        public void RemoveState_Does_Nothign_If_The_State_Is_Not_Defined()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");

            fsm.RemoveState(state);

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void RemoveState_Throws_Exception_If_AssetRepository_Is_Null()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");
            fsm.TryAddState(state);
            Assert.Throws<NullReferenceException>(() => fsm.RemoveState(state));
        }

        [Test]
        public void RemoveState_Removes_The_State()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.AssetRepository = new NoopAssetRepository();
            fsm.Init(null);

            FsmState state = GivenANoopInitializedState("SomeState");
            fsm.TryAddState(state);
            fsm.RemoveState(state);

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void RemoveState_Leaves_The_State_Machine_With_Exactly_One_State_Less()
        {
            FiniteStateMachine fsm = GivenAnEmptyFiniteStateMachine();
            fsm.AssetRepository = new NoopAssetRepository();
            fsm.Init(null);

            FsmState stateA = GivenANoopInitializedState("StateA");
            FsmState stateB = GivenANoopInitializedState("StateB");
            FsmState stateC = GivenANoopInitializedState("StateC");

            fsm.TryAddState(stateA);
            fsm.TryAddState(stateB);
            fsm.TryAddState(stateC);

            fsm.RemoveState(stateB);

            Assert.AreEqual(2, fsm.States.Count);
        }
    }

    public class StartTests
    {
        [Test]
        public void Start_Without_InitialState_Does_Nothing()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            fsm.Start();

            Assert.IsNull(fsm.ActiveState);
        }

        [Test]
        public void Start_Set_ActiveState()
        {
            FsmState stateA = GivenANoopInitializedState("StateA");
            FsmState stateB = GivenANoopInitializedState("StateB");

            FiniteStateMachine fsm = GivenAFiniteStateMachine(stateA);

            fsm.Start();

            Assert.True(stateA.IsEquals(fsm.ActiveState));
        }
    }

    public class StopTests
    {
        [Test]
        public void Stop_Without_InitialState_Does_Nothing()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            fsm.Stop();

            Assert.IsNull(fsm.ActiveState);
        }

        [Test]
        public void Stop_Set_ActiveState_To_Null()
        {
            FsmState stateA = GivenANoopInitializedState("StateA");
            FsmState stateB = GivenANoopInitializedState("StateB");

            FiniteStateMachine fsm = GivenAFiniteStateMachine(stateA);

            fsm.Start();
            fsm.Stop();

            Assert.IsNull(fsm.ActiveState);
        }

    }

    public class UpdateTests
    {
        [Test]
        public void Update_Initial_State_Not_Initialized_Throws_Exception()
        {
            CounterFsmState counterState = GivenACounterState("CounterState");

            FiniteStateMachine fsm = GivenAFiniteStateMachine(counterState);

            fsm.Start();
            Assert.Throws<NullReferenceException>(() => fsm.Update());

        }

        [Test]
        public void Update_Executes_ActiveState()
        {
            CounterFsmState counterState = GivenACounterInitializedState("CounterState");

            FiniteStateMachine fsm = GivenAFiniteStateMachine(counterState);

            Assert.AreEqual(0, counterState.Counter);

            fsm.Start();
            fsm.Update();

            Assert.AreEqual(1, counterState.Counter);
        }

        [Test]
        public void Update_For_A_Non_Started_Fsm_Leaves_ActiveState_Null()
        {
            CounterFsmState counterState = GivenACounterInitializedState("CounterState");

            FiniteStateMachine fsm = GivenAFiniteStateMachine(counterState);

            fsm.Update();

            Assert.IsNull(fsm.ActiveState);
        }

        [Test]
        public void Update_Returns_State_With_Valid_Transition_For_A_FSM_With_2_States()
        {
            CounterFsmState initialState = GivenACounterInitializedState("InitialState");
            NoopFsmState targetState = GivenANoopInitializedState("TargetState");

            FsmTransition initialToTargetTransition = GivenAnAlwaysValidTransition(targetState);
            initialState.AddTransition(initialToTargetTransition);

            FsmTransition targetToSelfTransition = GivenAnAlwaysValidTransition(targetState);
            targetState.AddTransition(targetToSelfTransition);

            FiniteStateMachine fsm = GivenAFiniteStateMachine(initialState);
            fsm.TryAddState(targetState);

            fsm.Start();
            fsm.Update();
            Assert.AreEqual(1, initialState.Counter);
            Assert.True(fsm.ActiveState.IsEquals(targetState));
        }

        [Test]
        public void Update_Calls_Enter_Function_Of_The_State_We_Transition_To()
        {
            CounterFsmState initialState = GivenACounterInitializedState("InitialState");
            NoopFsmState targetState = GivenANoopInitializedState("TargetState");

            FsmTransition initialToTargetTransition = GivenAnAlwaysValidTransition(targetState);
            initialState.AddTransition(initialToTargetTransition);

            FsmTransition targetToSelfTransition = GivenAnAlwaysValidTransition(targetState);
            targetState.AddTransition(targetToSelfTransition);

            FiniteStateMachine fsm = GivenAFiniteStateMachine(initialState);
            fsm.TryAddState(targetState);

            fsm.Start();
            fsm.Update();
            Assert.AreEqual(1, targetState.EnterCounter);
        }

        [Test]
        public void Update_Calls_Exit_Function_Of_The_State_We_Are_Leaving()
        {
            CounterFsmState initialState = GivenACounterInitializedState("InitialState");
            NoopFsmState targetState = GivenANoopInitializedState("TargetState");

            FsmTransition initialToTargetTransition = GivenAnAlwaysValidTransition(targetState);
            initialState.AddTransition(initialToTargetTransition);

            FsmTransition targetToSelfTransition = GivenAnAlwaysValidTransition(targetState);
            targetState.AddTransition(targetToSelfTransition);

            FiniteStateMachine fsm = GivenAFiniteStateMachine(initialState);
            fsm.TryAddState(targetState);

            fsm.Start();
            fsm.Update();
            Assert.AreEqual(1, initialState.ExitCounter);
        }

        [Test]
        public void Update_Returns_State_With_Valid_Transition_For_A_Fsm_With_3_States()
        {
            CounterFsmState initialState = GivenACounterInitializedState("InitialState");
            NoopFsmState stateA = GivenANoopInitializedState("StateA");
            NoopFsmState stateB = GivenANoopInitializedState("StateB");

            FsmTransition toStateATransition = GivenAnAlwaysInvalidTransition(stateA);
            initialState.AddTransition(toStateATransition);

            FsmTransition toStateBTransition = GivenAnAlwaysValidTransition(stateB);
            initialState.AddTransition(toStateBTransition);

            FiniteStateMachine fsm = GivenAFiniteStateMachine(initialState);
            fsm.TryAddState(stateA);
            fsm.TryAddState(stateB);

            fsm.Start();
            fsm.Update();

            Assert.True(fsm.ActiveState.IsEquals(stateB));
        }
    }

    public class TryAddTransitionTests
    {
        [Test]
        public void TryAddTransition_Adds_The_Transition()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            fsm.AssetRepository = GivenAnAssetRepository();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("ToState");

            fsm.TryAddState(fromState);
            fsm.TryAddState(toState);

            Assert.True(fsm.TryAddTransition(fromState, toState));
            Assert.True(fsm.HasTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryAddTransition_For_Non_Existing_States_Does_Not_Create_The_Transition()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("ToState");

            Assert.False(fsm.TryAddTransition(fromState, toState));
            Assert.False(fsm.HasTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryAddTransition_And_From_State_Does_Not_Exists_Does_Not_Create_The_Transition()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("ToState");

            fsm.TryAddState(fromState);

            Assert.False(fsm.TryAddTransition(fromState, toState));
            Assert.False(fsm.HasTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryAddTransition_And_To_State_Does_Not_Exists_Does_Not_Create_The_Transition()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("ToState");

            fsm.TryAddState(toState);
            fsm.TryAddTransition(fromState, toState);

            Assert.False(fsm.HasTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryAddTransition_For_An_Empty_State_Machine_Returs_False()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("ToState");

            Assert.False(fsm.TryAddTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }
    }

    public class TryRemoveTransitionTest
    {
        [Test]
        public void TryRemoveTransition_Removes_The_Transition()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            fsm.AssetRepository = GivenAnAssetRepository();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("toState");

            fsm.TryAddState(fromState);
            fsm.TryAddState(toState);

            fsm.TryAddTransition(fromState, toState);

            Assert.True(fsm.TryRemoveTransition(fromState, toState));
            Assert.False(fsm.HasTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryRemoveTransition_And_ToState_Does_Not_Exits_Retuns_False()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("toState");

            fsm.TryAddState(fromState);
            fsm.TryAddTransition(fromState, toState);

            Assert.False(fsm.TryRemoveTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryRemoveTransition_And_FromState_Does_Not_Exits_Retuns_False()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("toState");

            fsm.TryAddState(toState);
            fsm.TryAddTransition(fromState, toState);

            Assert.False(fsm.TryRemoveTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }

        [Test]
        public void TryRemoveTransition_For_An_Empty_StateMachine_Retuns_False()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("toState");

            fsm.TryAddTransition(fromState, toState);

            Assert.False(fsm.TryRemoveTransition(fromState, toState));

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }
    }

    public class GetReachableStatesTests
    {
        [Test]
        public void GetReachableStates_Returns_The_All_The_States_We_Can_Transition_To_For_A_Given_State()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            fsm.AssetRepository = GivenAnAssetRepository();

            FsmState fromState = GivenAStateBase("FromState");
            FsmState toState = GivenAStateBase("toState");

            fsm.TryAddState(fromState);
            fsm.TryAddState(toState);

            fsm.TryAddTransition(fromState, toState);

            List<FsmState> states = fsm.GetReachableStates(fromState);
            Assert.IsNotEmpty(states);

            ScriptableObject.DestroyImmediate(fromState);
            ScriptableObject.DestroyImmediate(toState);
        }
    }

    public class CloneTests
    {
        [Test]
        public void Clone_Clones_An_FSM_With_2_States()
        {
            CounterFsmState initialState = GivenACounterInitializedState("InitialState");
            NoopFsmState targetState = GivenANoopInitializedState("TargetState");

            FsmTransition initialToTargetTransition = GivenAnAlwaysValidTransition(targetState);
            initialState.AddTransition(initialToTargetTransition);

            FsmTransition targetToSelfTransition = GivenAnAlwaysValidTransition(targetState);
            targetState.AddTransition(targetToSelfTransition);

            FiniteStateMachine fsm = GivenAFiniteStateMachine(initialState);
            fsm.TryAddState(targetState);

            FiniteStateMachine clone = fsm.Clone();

            Assert.NotNull(clone);
            Assert.IsTrue(clone.name.Contains("Clone"));

            foreach (FsmState state in clone.States)
            {
                Assert.IsTrue(state.name.Contains("Clone"));
            }
        }
    }

    private static IAssetRepository GivenAnAssetRepository()
    {
        return new NoopAssetRepository();
    }

    private static IUndoRedoUtility GivenAnUndoRedoUtility()
    {
        return new NoopUndoRedoUtility();
    }

    private static FsmState GivenAStateBase(string stateName)
    {
        FsmState stateBase = ScriptableObject.CreateInstance<FsmState>();
        stateBase.StateName = stateName;
        return stateBase;
    }

    private static NoopFsmState GivenANoopInitializedState(string stateName)
    {
        NoopFsmState state = ScriptableObject.CreateInstance<NoopFsmState>();
        state.StateName = stateName;
        return state;
    }

    private static CounterFsmState GivenACounterState(string stateName)
    {
        CounterFsmState state = ScriptableObject.CreateInstance<CounterFsmState>();
        state.StateName = stateName;
        return state;
    }

    private static CounterFsmState GivenACounterInitializedState(string stateName)
    {
        CounterFsmState state = ScriptableObject.CreateInstance<CounterFsmState>();
        state.StateName = stateName;
        state.Transitions = new List<FsmTransition>();
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

    private static FiniteStateMachine GivenAnEmptyFiniteStateMachine()
    {
        FiniteStateMachine fsm = ScriptableObject.CreateInstance<FiniteStateMachine>();
        return fsm;
    }

    private static FiniteStateMachine GivenAFiniteStateMachine(FsmState initialState)
    {
        FiniteStateMachine fsm = ScriptableObject.CreateInstance<FiniteStateMachine>();
        fsm.Init(initialState);
        return fsm;
    }
}
