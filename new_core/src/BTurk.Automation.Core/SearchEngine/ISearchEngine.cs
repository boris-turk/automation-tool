using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchEngine
    {
        string SearchText { get; set; }

        string FilterText { get; set; }

        Selection TextSelection { get; set; }

        SearchItem SelectedItem { get; }

        ActionType ActionType { get; }

        void AddItems(IEnumerable<SearchItem> items);
    }
}