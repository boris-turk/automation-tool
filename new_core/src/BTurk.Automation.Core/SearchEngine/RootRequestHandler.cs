using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    public class RootRequestHandler : IRequestHandler<Request>
    {
        private readonly List<Request> _requests;
        private readonly IRequestHandler<CompositeRequest> _requestHandler;

        public RootRequestHandler(IRequestHandler<CompositeRequest> requestHandler)
        {
            _requests = new List<Request>();
            _requestHandler = requestHandler;
        }

        protected void AddRequest(Request rule)
        {
            _requests.Add(rule);
        }

        public void Handle(Request request)
        {
            if (!_requests.Any())
                return;

            var compositeRequest = new CompositeRequest(_requests);

            _requestHandler.Handle(compositeRequest);

            if (compositeRequest.HandledRequest == _requests.FirstOrDefault())
                request.Handled = true;
        }
    }
}