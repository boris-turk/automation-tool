// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.SearchEngine
{
    public interface IRequestHandler<TRequest> where TRequest : Request
    {
        void Handle(TRequest request);
    }
}
