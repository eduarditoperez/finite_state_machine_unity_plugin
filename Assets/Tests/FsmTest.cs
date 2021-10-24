using Fsm.Core;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fsm.State;

public class FsmTest
{
    public class ConstructorTests
    {
        [Test]
        public void Constructor_Creates_Empty_States()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void Constructor_Doesnt_Have_An_ActiveState()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            Assert.IsNull(fsm.ActiveState);
        }

        [Test]
        public void Constructor_Doesnt_Have_An_InitialState()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            Assert.IsNull(fsm.InitialState);
        }

        [Test]
        public void Constructor_With_Initial_State_Set_InitialState()
        {
            FsmState initialState = new NoopFsmState("initialState");
            FiniteStateMachine fsm = new FiniteStateMachine(initialState);

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
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState state = new NoopFsmState("SomeState");

            fsm.AddState(state);

            Assert.IsNotEmpty(fsm.States);
        }

        [Test]
        public void AddState_Doesnt_Add_The_State_Twice()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState state = new NoopFsmState("SomeState");

            fsm.AddState(state);
            fsm.AddState(state);

            Assert.AreEqual(1, fsm.States.Count);
        }

        [Test]
        public void AddState_Doesnt_Adds_Different_States()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState stateA = new NoopFsmState("StateA");
            FsmState stateB = new NoopFsmState("StateB");

            fsm.AddState(stateA);
            fsm.AddState(stateB);

            Assert.AreEqual(2, fsm.States.Count);
        }
    }

    public class RemoveStateTests
    {
        [Test]
        public void RemoveState_Does_Nothign_If_The_State_Is_Not_Defined()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState state = new NoopFsmState("SomeState");

            fsm.RemoveState(state);

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void RemoveState_Removes_The_State()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState state = new NoopFsmState("SomeState");
            fsm.AddState(state);
            fsm.RemoveState(state);

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void RemoveState_Leaves_The_State_Machine_With_Exactly_One_State_Less()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmState stateA = new NoopFsmState("StateA");
            FsmState stateB = new NoopFsmState("StateB");
            FsmState stateC = new NoopFsmState("StateC");

            fsm.AddState(stateA);
            fsm.AddState(stateB);
            fsm.AddState(stateC);

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
            FsmState stateA = new NoopFsmState("StateA");
            FsmState stateB = new NoopFsmState("StateB");

            FiniteStateMachine fsm = new FiniteStateMachine(stateA);

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
            FsmState stateA = new NoopFsmState("StateA");
            FsmState stateB = new NoopFsmState("StateB");

            FiniteStateMachine fsm = new FiniteStateMachine(stateA);

            fsm.Start();
            fsm.Stop();

            Assert.IsNull(fsm.ActiveState);
        }
    }

    public class UpdateTests
    {
        [Test]
        public void Update_Executes_ActiveState()
        {
            CounterFsmState counterState = new CounterFsmState("CounterState");

            FiniteStateMachine fsm = new FiniteStateMachine(counterState);

            Assert.AreEqual(0, counterState.Counter);

            fsm.Start();
            fsm.Update();

            Assert.AreEqual(1, counterState.Counter);
        }
    }
}
