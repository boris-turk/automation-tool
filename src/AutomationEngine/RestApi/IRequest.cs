// ReSharper disable UnusedTypeParameter

namespace AutomationEngine.RestApi
{
    public interface IRequest
    {
        string EndPointPath { get; }
    }

    public interface IRequest<TResult> : IRequest
    {
    }
}