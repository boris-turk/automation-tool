// ReSharper disable TypeParameterCanBeVariant

using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestVisitor
    {
        void Visit(Request request, ActionType actionType);
    }

    public interface IRequestVisitor<TRequest> where TRequest : Request
    {
        void Visit(TRequest request, ActionType actionType);
    }
}