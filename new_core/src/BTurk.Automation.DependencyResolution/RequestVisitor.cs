using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public class RequestVisitor : IRequestVisitor
    {
        void IRequestVisitor.Visit(IRequest request, ActionType actionType)
        {
            GenericMethodInvoker.Instance(this)
                .Method(nameof(Visit))
                .WithGenericTypes(request.GetType())
                .WithArguments(request, actionType)
                .Invoke();
        }

        public void Visit<TRequest>(TRequest request, ActionType actionType) where TRequest : IRequest
        {
            var visitor = Container.GetInstance<IRequestVisitor<TRequest>>();
            visitor.Visit(request, actionType);
        }
    }
}