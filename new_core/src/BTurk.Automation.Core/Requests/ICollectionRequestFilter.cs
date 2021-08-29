namespace BTurk.Automation.Core.Requests
{
    public interface ICollectionRequestFilter<in TRequest> : ICollectionRequest<TRequest> where TRequest : IRequest
    {
        bool CanLoad(TRequest request);
    }
}