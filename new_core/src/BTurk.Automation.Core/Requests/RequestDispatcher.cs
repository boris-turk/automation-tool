using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [Serializable]
    public class RequestDispatcher
    {
        private List<Request> _activeRequests;
        private readonly List<Request> _rootRequests;
        private readonly ISearchItemsProvider _searchItemsProvider;
        private readonly IEnvironmentContextProvider _environmentContextProvider;

        public RequestDispatcher(List<Request> rootRequests, ISearchItemsProvider searchItemsProvider,
            IEnvironmentContextProvider environmentContextProvider)
        {
            _activeRequests = new List<Request>();

            _rootRequests = rootRequests;
            _searchItemsProvider = searchItemsProvider;
            _environmentContextProvider = environmentContextProvider;
        }

        private List<Request> SearchItems => _searchItemsProvider.Items;

        public void Reset()
        {
            _activeRequests.Clear();
        }

        public void Dispatch()
        {
            if (!_activeRequests.Any())
                _activeRequests.Add(GetActiveRootRequest());

            if (!_activeRequests.Any())
                return;

            SearchItems.Clear();

            SearchItems.AddRange(GetCurrentSearchContents());
        }

        private IEnumerable<Request> GetCurrentSearchContents()
        {
            var activeRequest = _activeRequests.Last();
            return GetChildRequests(activeRequest);
        }

        private Request GetActiveRootRequest()
        {
            var request = _rootRequests.FirstOrDefault(_ => GetChildRequests(_).Any());
            return request;
        }

        private IEnumerable<Request> GetChildRequests(Request request)
        {
            return request.ChildRequests(_environmentContextProvider.Context);
        }
    }
}
