using System;
using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class MainSearchHandler : ISearchHandler
    {
        private ISearchHandler _activeHandler;
        private readonly List<ISearchHandler> _searchHandlers;

        public MainSearchHandler(List<ISearchHandler> searchHandlers)
        {
            _searchHandlers = searchHandlers;
        }

        public SearchResultsCollection Handle(SearchParameters parameters)
        {
            if (_activeHandler != null)
            {
                var result = _activeHandler.Handle(parameters);

                if (result.IsActive)
                    return result;
            }

            var allSearchItems = new List<SearchItem>();

            foreach (var handler in _searchHandlers)
            {
                var result = handler.Handle(parameters);

                if (result.IsActive)
                {
                    _activeHandler = handler;
                    return result;
                }

                allSearchItems.AddRange(result.Items);
            }

            return new SearchResultsCollection(allSearchItems);
        }
    }
}
