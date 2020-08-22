using System;
using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class MainSearchHandler : ISearchHandler, ISearchHandlersCollection
    {
        private readonly List<ISearchHandler> _handlers = new List<ISearchHandler>();
        private ISearchHandler _activeHandler;

        public void AddHandler(ISearchHandler handler)
        {
            _handlers.Add(handler);
        }

        public void RemoveHandler(ISearchHandler handler)
        {
            _handlers.Remove(handler);
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

            foreach (var handler in _handlers)
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