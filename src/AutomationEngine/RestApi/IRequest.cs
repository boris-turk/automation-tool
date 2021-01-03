namespace AutomationEngine.RestApi
{
    public interface IRequest
    {
    }

    public interface IRequest<TResult> : IRequest
    {
    }
}