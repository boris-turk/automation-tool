using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestVisitContext
{
    public RequestVisitContext(IRequest request, IRequest childRequest, ActionType actionType)
    {
        Request = request;
        ChildRequest = childRequest;
        ActionType = actionType;
    }

    public IRequest Request { get; }

    public IRequest ChildRequest { get; }

    public ActionType ActionType { get; }

    public RequestVisitContext<TRequest, TChild> CastTo<TRequest, TChild>()
        where TRequest : IRequest where TChild : IRequest
    {
        return new((TRequest)Request, (TChild)ChildRequest, ActionType);
    }
}

public class RequestVisitContext<TRequest, TChild>
    where TRequest : IRequest
    where TChild : IRequest
{
    public RequestVisitContext(TRequest request, TChild childRequest, ActionType actionType)
    {
        Request = request;
        ChildRequest = childRequest;
        ActionType = actionType;
    }

    public TRequest Request { get; }

    public TChild ChildRequest { get; set; }

    public ActionType ActionType { get; }
}