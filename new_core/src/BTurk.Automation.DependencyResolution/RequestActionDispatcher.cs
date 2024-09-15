using System.Diagnostics;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

[DebuggerStepThrough]
public class RequestActionDispatcher : IRequestActionDispatcher
{
    public RequestActionDispatcher(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

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
        var dispatcher = Container.GetInstance<IRequestActionDispatcher<TRequest>>();
        dispatcher.Dispatch(request, actionType);
    }
}