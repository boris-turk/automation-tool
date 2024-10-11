using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public interface ISearchEngineV2
{
    EnvironmentContext Context { get; }
    IRequestV2 RootMenuRequest { get; }
    List<SearchToken> SearchTokens { get; set; }
    void SetSearchResults(List<SearchResult> resultsCollection);
}