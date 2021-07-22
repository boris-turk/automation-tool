using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.DependencyResolution
{
    public class RequestVisitor : IRequestVisitor
    {
        void IRequestVisitor.Visit(Request request)
        {
            GenericMethodInvoker.Instance(this)
                .Method(nameof(Visit))
                .WithGenericTypes(request.GetType())
                .WithArguments(request)
                .Invoke();
        }

        public void Visit<TRequest>(TRequest request) where TRequest : Request
        {
            var visitor = Container.GetInstance<IRequestVisitor<TRequest>>();
            visitor.Visit(request);
        }
    }
}