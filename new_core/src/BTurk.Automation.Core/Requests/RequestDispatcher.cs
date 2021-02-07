using System;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [Serializable]
    public class RequestDispatcher
    {
        private readonly ISearchItemsProvider _searchItemsProvider;

        public RequestDispatcher(ISearchItemsProvider searchItemsProvider)
        {
            _searchItemsProvider = searchItemsProvider;
        }

        public void Dispatch()
        {
        }
    }
}
