using System;
using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class MainSearchHandler : ISearchHandler
    {
        private ISearchHandler _activeHandler;
        private readonly ISearchItemsProvider _searchItemsProvider;
        private readonly List<ISearchHandler> _searchHandlers;

        public MainSearchHandler(ISearchItemsProvider searchItemsProvider, List<ISearchHandler> searchHandlers)
        {
            _searchItemsProvider = searchItemsProvider;
            _searchHandlers = searchHandlers;
        }

        public void Handle(Request request)
        {
            if (_activeHandler != null)
            {
                _activeHandler.Handle(request);

                if (request.Handled)
                    return;
            }

            foreach (var handler in _searchHandlers)
            {
                var items = _searchItemsProvider.Items.ToList();

                handler.Handle(request);

                if (request.Handled)
                {
                    _searchItemsProvider.Items.RemoveAll(_ => items.Contains(_));
                    _activeHandler = handler;
                    return;
                }
            }
        }
    }
}
