namespace Fsm.State.Transition
{
    public class AlwaysTrueValidator : ValidatorStrategy
    {
        public bool IsValid()
        {
            return true;
        }
    }
}
