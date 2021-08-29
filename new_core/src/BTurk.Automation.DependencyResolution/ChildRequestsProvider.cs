using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public class ChildRequestsProvider : IChildRequestsProvider
    {
        private readonly IEnvironmentContextProvider _environmentContextProvider;

        public ChildRequestsProvider(IEnvironmentContextProvider environmentContextProvider)
        {
            _environmentContextProvider = environmentContextProvider;
        }

        public IEnumerable<IRequest> LoadChildren(IRequest request)
        {
            if (request is ICollectionRequest selectionRequest)
                return selectionRequest.GetRequests(_environmentContextProvider.Context);

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

        private IEnumerable<Request> GetRequests<TRequest>(ICollectionRequest<TRequest> collectionRequest)
            where TRequest : Request
        {
            var provider = Container.GetInstance<IRequestsProvider<TRequest>>();

            foreach (var request in provider.GetRequests())
            {
                if (collectionRequest is ICollectionRequestFilter<TRequest> filter && !filter.CanLoad(request))
                    continue;

                collectionRequest.OnLoaded(request);

                yield return request;
            }
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
    }
}