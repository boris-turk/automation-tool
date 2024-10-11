using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

public class ChildRequestsProvider : IChildRequestsProvider, IChildRequestsProviderV2
{
    public ChildRequestsProvider(Container container, IEnvironmentContextProvider environmentContextProvider)
    {
        Container = container;
        EnvironmentContextProvider = environmentContextProvider;
    }

    private Container Container { get; }

    private IEnvironmentContextProvider EnvironmentContextProvider { get; }

    public IEnumerable<IRequest> LoadChildren(IRequest request)
    {
        var collectionElementType = GetCollectionElementType(request);

        if (collectionElementType == null)
            return Enumerable.Empty<Request>();

        var childRequests = GenericMethodInvoker.Instance(this)
            .Method(nameof(GetRequests))
            .WithGenericTypes(collectionElementType)
            .WithArguments(request)
            .Invoke();

        return (IEnumerable<IRequest>)childRequests;
    }

    private IEnumerable<IRequest> GetRequests<TRequest>(ICollectionRequest<TRequest> collectionRequest)
        where TRequest : IRequest
    {
        var provider = Container.GetInstance<IRequestsProvider<TRequest>>();

        var suppliedRequests = provider.GetRequests().ToList();
        var environmentContext = EnvironmentContextProvider.Context;
        var context = new RequestLoadContext<TRequest>(environmentContext, suppliedRequests);

        return collectionRequest.GetRequests(context);
    }

    private Type GetCollectionElementType(IRequest request)
    {
        var parentType = request.GetType();

        if (!parentType.InheritsFrom(typeof(ICollectionRequest<>)))
            return null;

        var closedGenericType = parentType.FindAllParentClosedGenerics(typeof(ICollectionRequest<>)).Single();

        var collectionElementType = closedGenericType.GetGenericArguments()[0];

        return collectionElementType;
    }

    IEnumerable<IRequestV2> IChildRequestsProviderV2.LoadChildren(IRequestV2 request)
    {
        return LoadChildren((IRequest)request);
    }
}