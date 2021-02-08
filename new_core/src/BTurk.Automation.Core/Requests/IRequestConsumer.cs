// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestConsumer<TRequest> where TRequest : Request
    {
        void Execute(TRequest request);
    }
}