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
            var parentConsumerType = GetParentConsumerType(request);

            if (parentConsumerType == null)
                return request.ChildRequests(_contextProvider.Context);

            var result = GenericMethodInvoker.Instance(this)
                .Method(nameof(LoadChildren))
                .WithGenericTypes(parentConsumerType)
                .Invoke();

            return (IEnumerable<Request>)result;
        }

        public IEnumerable<Request> LoadChildren<TRequest>() where TRequest : Request
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

            if (context.ParentRequests.LastOrDefault() is IRequestConsumer<TRequest> consumer)
            {
                consumer.Execute(properContext.Request);
            }
            else
            {
                var executor = Container.GetInstance<IRequestExecutor<TRequest>>();
                executor.Execute(properContext);
            }
        }

        private Type GetParentConsumerType(Request request)
        {
            var parentInterfaces = request.GetType().FindAllParentClosedGenerics(typeof(IRequestConsumer<>));
            var singleInterface = parentInterfaces.SingleOrDefault();
            return singleInterface?.GetGenericArguments()[0];
        }
    }
}