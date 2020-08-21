using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    public class MainSearchHandler : ISearchHandler, ISearchHandlersCollection
    {
        private readonly List<ISearchHandler> _handlers = new List<ISearchHandler>();

        public void AddHandler(ISearchHandler handler)
        {
            _handlers.Add(handler);
        }

        public void RemoveHandler(ISearchHandler handler)
        {
            _handlers.Remove(handler);
        }

        public SearchResult Handle(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return SearchResult.Empty;

            foreach (var handler in _handlers)
            {
                var result = handler.Handle(text);

                if (result != SearchResult.Empty)
                    return result;
            }

            return SearchResult.Empty;
        }
    }
}