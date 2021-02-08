using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchEngine : IEnvironmentContextProvider, ISearchItemsProvider
    {
        string SearchText { get; set; }

        Selection TextSelection { get; set; }

        Request SelectedItem { get; }

        ActionType ActionType { get; }

        void AddItems(IEnumerable<Request> items);
    }
}