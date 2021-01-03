namespace AutomationEngine.RestApi
{
    public interface IRestClient
    {
        TResult SendRequest<TRequest, TResult>(TRequest request) where TRequest : IRequest<TResult>;
    }
}