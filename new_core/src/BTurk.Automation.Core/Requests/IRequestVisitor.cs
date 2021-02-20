// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestVisitor
    {
        void Visit(Request request);
    }

    public interface IRequestVisitor<TRequest> where TRequest : Request
    {
        void Visit(TRequest request);
    }
}