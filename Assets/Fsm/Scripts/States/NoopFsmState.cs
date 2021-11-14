namespace Fsm.State
{
    /// <summary>
    /// Placeholder state
    /// </summary>
    public class NoopFsmState : State
    {
        //public NoopFsmState(string stateName) : base(stateName) {}

        public override void Enter() {}

        public override void Exit() {}

        public override State Update()
        {
            return this;
        }
    }
}