namespace Fsm.Executor
{
    public interface IInternalStateExecutor
    {
        void Start();
        void Stop();
        InternalStateExecutorResult Execute();
    }
}
