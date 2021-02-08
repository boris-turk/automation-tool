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
            var childRequestType = GetChildRequestType(request);

            if (childRequestType == null)
                return request.ChildRequests(_contextProvider.Context);

            var result = GenericMethodInvoker.Instance(this)
                .Method(nameof(LoadChildren))
                .WithGenericTypes(childRequestType)
                .Invoke();

            return (IEnumerable<Request>)result;
        }

        public IEnumerable<Request> LoadChildren<TRequest>() where TRequest : Request
        {
            var service = Container.GetInstance<IRequestsProvider<TRequest>>();
            return service.Load();
        }

        public void Execute(List<Request> currentRequests)
        {
            var requests = Enumerable.Reverse(currentRequests.ToList()).Take(2).ToList();

            if (!requests.Any())
                return;

            if (requests[0].Action != null)
            {
                requests[0].Action.Invoke();
                return;
            }

            if (requests.Count == 1)
                return;

            var childRequestType = GetChildRequestType(requests[1]);

            if (childRequestType != requests[0].GetType())
                return;

            GenericMethodInvoker.Instance(this)
                .Method(nameof(Execute))
                .WithGenericTypes(childRequestType)
                .WithArguments(requests[1], requests[0])
                .Invoke();
        }

        private void Execute<TChild>(IRequestConsumer<TChild> consumer, TChild childRequest)
            where TChild : Request
        {
            consumer.Execute(childRequest);
        }

        private Type GetChildRequestType(Request request)
        {
            var parentInterfaces = request.GetType().FindAllParentClosedGenerics(typeof(IRequestConsumer<>));
            var singleInterface = parentInterfaces.SingleOrDefault();
            return singleInterface?.GetGenericArguments()[0];
        }
    }
}