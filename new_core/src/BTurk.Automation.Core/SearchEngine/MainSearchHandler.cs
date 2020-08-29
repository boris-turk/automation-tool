using System;
using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class MainSearchHandler : ISearchHandler<Request>
    {
        private ISearchHandler<Request> _activeHandler;
        private readonly ISearchItemsProvider _searchItemsProvider;
        private readonly List<ISearchHandler<Request>> _searchHandlers;

        public MainSearchHandler(ISearchItemsProvider itemsProvider, List<ISearchHandler<Request>> searchHandlers)
        {
            _searchItemsProvider = itemsProvider;
            _searchHandlers = searchHandlers;
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

            foreach (var handler in _searchHandlers)
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
