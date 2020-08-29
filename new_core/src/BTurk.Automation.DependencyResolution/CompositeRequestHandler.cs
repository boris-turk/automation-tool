using System;
using BTurk.Automation.Core.SearchEngine;

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

                GetHandler(request.GetType()).Handle((dynamic)request);

                if (request.Handled)
                    break;
            }
        }
    }
}