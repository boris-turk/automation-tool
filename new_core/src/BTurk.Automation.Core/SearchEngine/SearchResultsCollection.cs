using System;
using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class SearchResultsCollection
    {
        public bool IsActive { get; set; }

        public List<SearchItem> Items { get; } = new List<SearchItem>();

        private SearchResultsCollection(SearchItem item)
            : this(new[] {item})
        {
        }

        public SearchResultsCollection(IEnumerable<SearchItem> items)
        {
            IsActive = true;
            Items.AddRange(items);
        }

        public static SearchResultsCollection Single(string name)
        {
            var item = new SearchItem { Text = name };
            return new SearchResultsCollection(item) { IsActive = false };
        }
    }
}