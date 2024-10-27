using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public interface ISearchEngineV2
{
    void Hide();
    EnvironmentContext Context { get; }
    IRequestV2 RootMenuRequest { get; }
    List<SearchToken> SearchTokens { get; set; }
    SearchResult SelectedSearchResult { get; }
    void SetSearchResults(List<SearchResult> resultsCollection);
}