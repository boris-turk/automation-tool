// ReSharper disable TypeParameterCanBeVariant

using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public interface IRequestActionDispatcher
{
    void Dispatch(IRequest request, ActionType actionType);
}

public interface IRequestActionDispatcher<TRequest> where TRequest : IRequest
{
    void Dispatch(TRequest request, ActionType actionType);
}