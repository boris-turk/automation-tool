using System.Diagnostics;
using BTurk.Automation.Core.Queries;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

[DebuggerStepThrough]
public class QueryProcessor : IQueryProcessor
{
    public QueryProcessor(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

    TResult IQueryProcessor.Process<TResult>(IQuery<TResult> query)
    {
        var result = GenericMethodInvoker.Instance(this)
            .Method(nameof(Handle))
            .WithGenericTypes(typeof(TResult), query.GetType())
            .WithArguments(query)
            .Invoke();

        return (TResult)result;
    }

    public object Handle<TResult, TQuery>(TQuery query) where TQuery : IQuery<TResult>
    {
        var handler = Container.GetInstance<IQueryHandler<TQuery, TResult>>();
        return handler.Handle(query);
    }
}