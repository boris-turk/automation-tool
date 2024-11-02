using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public interface ISearchEngine
{
    void Hide();
    EnvironmentContext Context { get; }
    IRequest RootMenuRequest { get; }
    List<SearchToken> SearchTokens { get; set; }
    SearchResult SelectedSearchResult { get; }
    void SetSearchResults(List<SearchResult> resultsCollection);
}