using System;
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
            foreach (var request in compositeRequest.Requests)
            {
                request.Handled = false;
                request.CanMoveNext = false;

                var handler = GetHandler(request.GetType());
                handler.Handle((dynamic)request);

                if (!request.Handled || !request.CanMoveNext)
                    break;
            }
        }
    }
}