using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IEnvironmentContextProvider _contextProvider;

        public RequestProcessor(IEnvironmentContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public IEnumerable<Request> LoadChildren(Request request)
        {
            var children = request.ChildRequests(_contextProvider.Context).ToList();

            if (children.Any())
                return children;

            var selectingRequestType = GetSelectingRequestType(request);

            if (selectingRequestType == null)
                return Enumerable.Empty<Request>();

            var result = GenericMethodInvoker.Instance(this)
                .Method(nameof(LoadRequests))
                .WithGenericTypes(selectingRequestType)
                .Invoke();

            return (IEnumerable<Request>)result;
        }

        public IEnumerable<Request> LoadRequests<TRequest>() where TRequest : Request
        {
            var service = Container.GetInstance<IRequestsProvider<TRequest>>();
            return service.Load();
        }

        public void Execute(RequestExecutionContext context)
        {
            GenericMethodInvoker.Instance(this)
                .Method(nameof(Execute))
                .WithGenericTypes(context.Request.GetType())
                .WithArguments(context)
                .Invoke();
        }

        private void Execute<TRequest>(RequestExecutionContext context) where TRequest : Request
        {
            var properContext = (RequestExecutionContext<TRequest>)context;

            if (context.ParentRequests.LastOrDefault() is SelectionRequest<TRequest> selectionRequest)
            {
                selectionRequest.Selected?.Invoke(properContext.Request);
            }
            else
            {
                var executor = Container.GetInstance<IRequestExecutor<TRequest>>();
                executor.Execute(properContext);
            }
        }

        private Type GetSelectingRequestType(Request request)
        {
            var parentInterfaces = request.GetType().FindAllParentClosedGenerics(typeof(SelectionRequest<>));
            var singleInterface = parentInterfaces.SingleOrDefault();
            return singleInterface?.GetGenericArguments()[0];
        }
    }
}