using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class SelectionRequestVisitor<TChild> : IRequestVisitor<SelectionRequest<TChild>, TChild>
    where TChild : IRequest
{
    public void Visit(RequestVisitContext<SelectionRequest<TChild>, TChild> context)
    {
        if (context.ActionType == ActionType.Complete)
            context.Request.ChildSelected?.Invoke(context.ChildRequest);

        if (context.ActionType == ActionType.StepBack)
            context.Request.ChildDeselected?.Invoke(context.ChildRequest);
    }
}