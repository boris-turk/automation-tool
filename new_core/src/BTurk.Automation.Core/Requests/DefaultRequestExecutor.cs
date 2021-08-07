namespace BTurk.Automation.Core.Requests
{
    public class DefaultRequestExecutor<TRequest> : IRequestExecutor<TRequest> where TRequest : IRequest
    {
        public void Execute(TRequest request) => request.Load();
    }
}