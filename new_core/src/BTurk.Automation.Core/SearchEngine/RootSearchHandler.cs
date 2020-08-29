using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    public class RootSearchHandler : ISearchHandler<Request>
    {
        private readonly List<Request> _requests;
        private readonly ISearchHandler<CompositeRequest> _searchHandler;

        public RootSearchHandler(ISearchHandler<CompositeRequest> searchHandler)
        {
            _requests = new List<Request>();
            _searchHandler = searchHandler;
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

            _searchHandler.Handle(compositeRequest);

            if (compositeRequest.HandledRequest == _requests.FirstOrDefault())
                request.Handled = true;
        }
    }
}