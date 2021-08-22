﻿using System;
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
            if (request is ISelectionRequest selectionRequest)
                return selectionRequest.GetRequests(_environmentContextProvider.Context);

            var childRequestType = GetChildRequestType(request);

            if (childRequestType == null)
                return Enumerable.Empty<Request>();

            var childRequests = GenericMethodInvoker.Instance(this)
                .Method(nameof(GetRequests))
                .WithGenericTypes(childRequestType)
                .Invoke();

            return (IEnumerable<Request>)childRequests;
        }

        private IEnumerable<Request> GetRequests<TRequest>() where TRequest : Request
        {
            var provider = Container.GetInstance<IRequestsProvider<TRequest>>();
            return provider.GetRequests();
        }

        private Type GetChildRequestType(IRequest request)
        {
            var parentType = request.GetType();

            if (!parentType.InheritsFrom(typeof(ISelectionRequest<>)))
                return null;

            var selectionRequestType = parentType.FindAllParentClosedGenerics(typeof(ISelectionRequest<>)).Single();

            var childType = selectionRequestType.GetGenericArguments()[0];

            return childType;
        }
    }
}