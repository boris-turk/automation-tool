using System;
using System.Linq;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.DependencyResolution
{
    public class CompositeRequestHandler : IRequestHandler<CompositeRequest>
    {
        private dynamic GetHandler(Type requestType)
        {
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
            return Container.GetInstance(handlerType);
        }

        public void Handle(CompositeRequest compositeRequest)
        {
            var requests = compositeRequest.Requests.ToList();

            requests.ForEach(_ => _.CanMoveNext = false);

            foreach (var request in requests)
            {
                var handler = GetHandler(request.GetType());
                handler.Handle((dynamic)request);

                if (!request.CanMoveNext)
                    break;
            }
        }
    }
}