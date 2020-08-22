using System;
using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class SearchResultsCollection
    {
        public bool IsActive { get; set; }

        public IEnumerable<SearchItem> Items { get; }

        private SearchResultsCollection(SearchItem item)
        {
            Items = new[] {item};
        }

        public SearchResultsCollection(IEnumerable<SearchItem> items)
        {
            Items = items;
        }

        public static SearchResultsCollection Single(string name)
        {
            var item = new SearchItem { Text = name };
            return new SearchResultsCollection(item);
        }
    }
}