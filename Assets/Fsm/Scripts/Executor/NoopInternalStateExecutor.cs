namespace Fsm.Executor
{
    public class NoopInternalStateExecutor : IInternalStateExecutor
    {
        public int StartCalls { get; private set; }
        public int StopCalls { get; private set; }
        public int ExecuteCalls { get; private set; }

        public NoopInternalStateExecutor()
        {
            StartCalls = 0;
            StopCalls = 0;
            ExecuteCalls = 0;
        }

        public InternalStateExecutorResult Execute()
        {
            ExecuteCalls++;
            return InternalStateExecutorResult.Success;
        }

        public void Start()
        {
            StartCalls++;
        }

        public void Stop()
        {
            StopCalls++;
        }

    }
}
