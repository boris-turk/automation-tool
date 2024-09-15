using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class OptionRequestVisitor<TChild> : IRequestVisitor<OptionRequest, TChild> where TChild : IRequest
{
    public void Visit(RequestVisitContext<OptionRequest, TChild> context)
    {
        if (context.ActionType == ActionType.Complete)
            context.Request.ChildSelected?.Invoke(context.ChildRequest);

        if (context.ActionType == ActionType.StepBack)
            context.Request.ChildDeselected?.Invoke(context.ChildRequest);
    }
}