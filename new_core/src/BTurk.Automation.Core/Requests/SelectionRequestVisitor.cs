using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequestVisitor<TChild> : IRequestVisitor<SelectionRequest<TChild>, TChild>
        where TChild : IRequest
    {
        public void Visit(RequestVisitContext<SelectionRequest<TChild>, TChild> context)
        {
            if (context.ActionType == ActionType.Execute)
                context.Request.ChildExecuted?.Invoke(context.ChildRequest);
        }
    }
}