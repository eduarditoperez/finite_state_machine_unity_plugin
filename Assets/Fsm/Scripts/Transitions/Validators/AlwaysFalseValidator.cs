namespace Fsm.State.Transition
{
    public class AlwaysFalseValidator : ValidatorStrategy
    {
        public bool IsValid()
        {
            return false;
        }
    }
}
