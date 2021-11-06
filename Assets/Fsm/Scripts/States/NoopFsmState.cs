namespace Fsm.State
{
    /// <summary>
    /// Placeholder state
    /// </summary>
    public class NoopFsmState : FsmStateBase
    {
        public NoopFsmState(string stateName) : base(stateName) {}

        public override void Enter() {}

        public override void Exit() {}

        public override FsmStateBase Update()
        {
            return this;
        }
    }
}