using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

public class ChildRequestsProvider : IChildRequestsProviderV2
{
    public ChildRequestsProvider(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

    IEnumerable<TRequest> IChildRequestsProviderV2.LoadChildren<TRequest>()
    {
        var provider = Container.GetInstance<IRequestsProvider<TRequest>>();
        var requests = provider.GetRequests().ToList();
        return requests;
    }
}