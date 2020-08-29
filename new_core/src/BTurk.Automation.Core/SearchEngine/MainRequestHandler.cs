using System;
using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class MainRequestHandler : IRequestHandler<Request>
    {
        private IRequestHandler<Request> _activeHandler;
        private readonly ISearchItemsProvider _searchItemsProvider;
        private readonly List<IRequestHandler<Request>> _requestHandlers;

        public MainRequestHandler(ISearchItemsProvider searchItemsProvider,
            List<IRequestHandler<Request>> requestHandlers)
        {
            _searchItemsProvider = searchItemsProvider;
            _requestHandlers = requestHandlers;
        }

        protected List<SearchItem> SearchItems => _searchItemsProvider.Items;

        public void Handle(Request request)
        {
            SearchItems.Clear();

            if (_activeHandler != null)
            {
                _activeHandler.Handle(request);

                if (request.Handled)
                    return;
            }

            SearchItems.Clear();

            foreach (var handler in _requestHandlers)
            {
                var items = SearchItems.ToList();

                handler.Handle(request);

                if (request.Handled)
                {
                    SearchItems.RemoveAll(_ => items.Contains(_));
                    _activeHandler = handler;
                    return;
                }
            }
        }
    }
}
