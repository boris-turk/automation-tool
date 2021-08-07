// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestExecutor<TRequest> where TRequest : IRequest
    {
        void Execute(TRequest request);
    }
}