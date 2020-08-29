using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchItemsProvider
    {
        List<SearchItem> Items { get; }
    }
}