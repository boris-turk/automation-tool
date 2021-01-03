namespace AutomationEngine.RestApi
{
    public interface IRequestParametersProvider
    {
        RequestParameters GetRequestParameters<TRequest>() where TRequest : IRequest;
    }
}