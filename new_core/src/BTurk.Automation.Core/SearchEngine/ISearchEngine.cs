using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public interface ISearchEngine : IEnvironmentContextProvider, ISearchItemsProvider
{
    List<SearchStep> Steps { get; }

    string SearchText { get; set; }

    Request SelectedItem { get; }

    void Hide();
}