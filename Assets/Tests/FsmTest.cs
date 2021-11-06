using Fsm.Core;
using NUnit.Framework;
using Fsm.State;
using Fsm.State.Transition;

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
            FsmStateBase initialState = new NoopFsmState("initialState");
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

            FsmStateBase state = new NoopFsmState("SomeState");

            fsm.AddState(state);

            Assert.IsNotEmpty(fsm.States);
        }

        [Test]
        public void AddState_Doesnt_Add_The_State_Twice()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmStateBase state = new NoopFsmState("SomeState");

            fsm.AddState(state);
            fsm.AddState(state);

            Assert.AreEqual(1, fsm.States.Count);
        }

        [Test]
        public void AddState_Doesnt_Adds_Different_States()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmStateBase stateA = new NoopFsmState("StateA");
            FsmStateBase stateB = new NoopFsmState("StateB");

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

            FsmStateBase state = new NoopFsmState("SomeState");

            fsm.RemoveState(state);

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void RemoveState_Removes_The_State()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmStateBase state = new NoopFsmState("SomeState");
            fsm.AddState(state);
            fsm.RemoveState(state);

            Assert.IsEmpty(fsm.States);
        }

        [Test]
        public void RemoveState_Leaves_The_State_Machine_With_Exactly_One_State_Less()
        {
            FiniteStateMachine fsm = new FiniteStateMachine();

            FsmStateBase stateA = new NoopFsmState("StateA");
            FsmStateBase stateB = new NoopFsmState("StateB");
            FsmStateBase stateC = new NoopFsmState("StateC");

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
            FsmStateBase stateA = new NoopFsmState("StateA");
            FsmStateBase stateB = new NoopFsmState("StateB");

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
            FsmStateBase stateA = new NoopFsmState("StateA");
            FsmStateBase stateB = new NoopFsmState("StateB");

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

        [Test]
        public void Update_For_A_Non_Started_Fsm_Leaves_ActiveState_Null()
        {
            CounterFsmState counterState = new CounterFsmState("CounterState");

            FiniteStateMachine fsm = new FiniteStateMachine(counterState);

            fsm.Update();

            Assert.IsNull(fsm.ActiveState);
        }

        [Test]
        public void Update_Returns_State_With_Valid_Transition_For_A_FSM_With_2_States()
        {
            CounterFsmState initialState = new CounterFsmState("InitialState");
            NoopFsmState targetState = new NoopFsmState("TargetState");

            FsmTransition initialToTargetTransition = new AlwaysValidTransition(targetState);
            initialState.AddTransition(initialToTargetTransition);

            FsmTransition targetToSelfTransition = new AlwaysValidTransition(targetState);
            targetState.AddTransition(targetToSelfTransition);

            FiniteStateMachine fsm = new FiniteStateMachine(initialState);
            fsm.AddState(targetState);

            fsm.Start();
            fsm.Update();
            Assert.AreEqual(1, initialState.Counter);
            Assert.True(fsm.ActiveState.IsEquals(targetState));
        }

        [Test]
        public void Update_Returns_State_With_Valid_Transition_For_A_Fsm_With_3_States()
        {
            CounterFsmState initialState = new CounterFsmState("InitialState");
            NoopFsmState stateA = new NoopFsmState("StateA");
            NoopFsmState stateB = new NoopFsmState("StateB");

            FsmTransition toStateATransition = new AlwaysInvalidTransition(stateA);
            initialState.AddTransition(toStateATransition);

            FsmTransition toStateBTransition = new AlwaysValidTransition(stateB);
            initialState.AddTransition(toStateBTransition);

            FiniteStateMachine fsm = new FiniteStateMachine(initialState);
            fsm.AddState(stateA);
            fsm.AddState(stateB);

            fsm.Start();
            fsm.Update();
            
            Assert.True(fsm.ActiveState.IsEquals(stateB));
        }
    }

    // TODO: add a test that changes the active state,
    // for this we need a fsm with a valid transition to 
    // another state
}
