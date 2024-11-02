using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public interface ISearchEngine : IEnvironmentContextProvider
{
    string SearchText { get; set; }

    Request SelectedItem { get; }

    void Hide();
}