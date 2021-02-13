namespace BTurk.Automation.Core.Requests
{
    public class EmptyRequestExecutor<TRequest> : IRequestExecutor<TRequest> where TRequest : Request
    {
        public void Execute(RequestExecutionContext<TRequest> context)
        {
        }
    }
}