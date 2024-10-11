using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestActionDispatcherV2 : IRequestActionDispatcherV2
{
    private readonly Dictionary<SearchToken, FilterAlgorithm> _filterAlgorithms = new();

    public RequestActionDispatcherV2(ISearchEngineV2 searchEngine, IChildRequestsProviderV2 childRequestsProvider)
    {
        SearchEngine = searchEngine;
        ChildRequestsProvider = childRequestsProvider;
    }

    private ISearchEngineV2 SearchEngine { [DebuggerStepThrough] get; }

    private IChildRequestsProviderV2 ChildRequestsProvider { get; }

    public void Dispatch(ActionType actionType)
    {
        if (actionType == ActionType.Search)
            Search();
    }

    private void Search()
    {
        var rootRequest = SearchEngine.RootMenuRequest;
        var searchTokens = SearchEngine.SearchTokens.ToList();

        var searchResults = new List<SearchResult> { new SearchResult().Append(rootRequest) };
        CollectSearchResults(searchResults, searchTokens);

        SearchEngine.SetSearchResults(searchResults);
    }

    private void CollectSearchResults(List<SearchResult> resultsCollection, List<SearchToken> searchTokens)
    {
        foreach (var result in resultsCollection.ToList())
        {
            var request = result.Items.Last().Request;

            if (request.Configuration.Text.HasLength() && searchTokens.Any() == false)
                continue;

            var childrenCollection = GetChildren(request, searchTokens).ToList();

            var insertIndex = resultsCollection.IndexOf(result);
            resultsCollection.Remove(result);

            foreach (var child in childrenCollection.OrderByDescending(c => c.Score))
            {
                resultsCollection.Insert(insertIndex, result.Append(child.Request));

                var oldCount = resultsCollection.Count;
                var childSearchTokens = GetChildSearchTokens(child, searchTokens);
                CollectSearchResults(resultsCollection, childSearchTokens);
                var newCount = resultsCollection.Count;

                insertIndex += newCount - oldCount + 1;
            }
        }
    }

    private List<SearchToken> GetChildSearchTokens((int Score, IRequestV2 Request) child, List<SearchToken> searchTokens)
    {
        if (child.Request.Configuration.Text.HasLength() == false)
            return searchTokens.ToList();

        if (child.Score == 1 && child.Request.Configuration.ScanChildrenIfUnmatched)
            return searchTokens.Skip(1).ToList();

        if (child.Score > 1000 && child.Request.Configuration.ScanChildrenIfUnmatched == false)
            return searchTokens.Skip(1).ToList();

        return searchTokens.ToList();
    }

    private IEnumerable<(int Score, IRequestV2 Request)> GetChildren(IRequestV2 request, List<SearchToken> searchTokens)
    {
        foreach (var child in request.Configuration.GetChildren(ChildRequestsProvider))
        {
            if (child.Configuration.CanProcess(SearchEngine.Context) == false)
                continue;

            var text = child.Configuration.Text;

            var score = GetScore(text, searchTokens);

            if (score > 0)
            {
                yield return (score, child);
                continue;
            }

            if (text.HasLength() == false)
            {
                yield return (Score: 1, child);
                continue;
            }

            if (searchTokens.Any() == false || child.Configuration.ScanChildrenIfUnmatched)
            {
                yield return (Score: 1, child);
            }
        }
    }

    private int GetScore(string text, List<SearchToken> searchTokensCollection)
    {
        if (searchTokensCollection.Any() == false)
            return 0;

        if (text.HasLength() == false)
            return 0;

        var searchToken = searchTokensCollection.First();

        if (_filterAlgorithms.TryGetValue(searchToken, out var filterAlgorithm) == false)
        {
            filterAlgorithm = new FilterAlgorithm(searchToken.Text);
            _filterAlgorithms[searchToken] = filterAlgorithm;
        }

        var score = filterAlgorithm.GetScore(text);

        return score;
    }
}