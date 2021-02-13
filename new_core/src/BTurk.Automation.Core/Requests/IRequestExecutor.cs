namespace BTurk.Automation.Core.Requests
{
    public interface IRequestExecutor<TRequest> where TRequest : Request
    {
        void Execute(RequestExecutionContext<TRequest> context);
    }
}