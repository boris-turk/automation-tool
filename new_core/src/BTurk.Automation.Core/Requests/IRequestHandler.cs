// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestHandler<TRequest> where TRequest : Request
    {
        void Handle(TRequest request);
    }
}
