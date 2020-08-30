// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestHandler<TRequest> where TRequest : IRequest
    {
        void Handle(TRequest request);
    }
}
