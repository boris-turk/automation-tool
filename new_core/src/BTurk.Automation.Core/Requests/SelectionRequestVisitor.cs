using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class SelectionRequestVisitor<TChild> : IRequestVisitor<SelectionRequest<TChild>, TChild>
    where TChild : IRequest
{
    public void Visit(RequestVisitContext<SelectionRequest<TChild>, TChild> context)
    {
        if (context.ActionType == ActionType.MoveNext)
            context.Request.ChildSelected?.Invoke(context.ChildRequest);

        if (context.ActionType == ActionType.MovePrevious)
            context.Request.ChildDeselected?.Invoke(context.ChildRequest);
    }
}