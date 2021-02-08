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

            var service = CreateRequestsProvider(childRequestType);
            var result = ((dynamic)service).Load();

            return (IEnumerable<Request>)result;
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
                .Method(nameof(ExecuteChildConsumer))
                .WithGenericTypes(childRequestType)
                .WithArguments(requests[1], requests[0])
                .Invoke();
        }

        private void ExecuteChildConsumer<TChild>(IRequestConsumer<TChild> consumer, TChild childRequest)
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

        private object CreateRequestsProvider(Type requestType)
        {
            var serviceType = typeof(IRequestsProvider<>).MakeGenericType(requestType);
            var instance = Container.GetInstance(serviceType);
            return instance;
        }
    }
}