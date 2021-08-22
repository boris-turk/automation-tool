using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class DefaultRequestVisitor<TRequest, TChild> : IRequestVisitor<TRequest, TChild>
        where TRequest : IRequest where TChild : IRequest
    {
        private readonly IRequestExecutor<TChild> _childRequestExecutor;

        public DefaultRequestVisitor(IRequestExecutor<TChild> childRequestExecutor)
        {
            _childRequestExecutor = childRequestExecutor;
        }

        public void Visit(RequestVisitContext<TRequest, TChild> context)
        {
            if (context.ActionType == ActionType.Execute)
                _childRequestExecutor.Execute(context.ChildRequest);
        }
    }
}