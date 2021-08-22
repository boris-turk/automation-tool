using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public class RequestActionDispatcher : IRequestActionDispatcher
    {
        void IRequestActionDispatcher.Dispatch(IRequest request, ActionType actionType)
        {
            GenericMethodInvoker.Instance(this)
                .Method(nameof(Dispatch))
                .WithGenericTypes(request.GetType())
                .WithArguments(request, actionType)
                .Invoke();
        }

        public void Dispatch<TRequest>(TRequest request, ActionType actionType) where TRequest : IRequest
        {
            var visitor = Container.GetInstance<IRequestActionDispatcher<TRequest>>();
            visitor.Dispatch(request, actionType);
        }
    }
}