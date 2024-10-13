using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestActionDispatcherV2 : IRequestActionDispatcherV2
{
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

            if (request.Configuration.CanHaveChildren == false && result.Items.Count == 1)
            {
                resultsCollection.Remove(result);
                break;
            }

            if (request.Configuration.CanHaveChildren == false && searchTokens.Any() == false)
            {
                continue;
            }

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
        const int sufficientMatch = 1;
        const int optimalMatch = 1000;

        var request = child.Request;
        var text = request.Configuration.Text;

        if (text.HasLength() == false)
            return searchTokens.ToList();

        var skipCount = child.Score switch
        {
            sufficientMatch when request.Configuration.ScanChildrenIfUnmatched == false => 1,
            sufficientMatch when searchTokens.All(x => x is SpaceToken) => 1,
            > optimalMatch when request.Configuration.CanHaveChildren => 1,
            > optimalMatch => searchTokens.TakeWhile(x => x is WordToken).Count(),
            _ => 0
        };

        return searchTokens.Skip(skipCount).ToList();
    }

    private IEnumerable<(int Score, IRequestV2 Request)> GetChildren(IRequestV2 request, List<SearchToken> searchTokens)
    {
        string singleWordSearchText = null;
        string multiWordSearchText = null;

        foreach (var child in request.Configuration.GetChildren(ChildRequestsProvider))
        {
            if (child.Configuration.CanProcess(SearchEngine.Context) == false)
                continue;

            var text = child.Configuration.Text;

            int score;

            if (child.Configuration.CanHaveChildren)
            {
                singleWordSearchText ??= GetSingleWordSearchText(searchTokens);
                score = GetScore(text, singleWordSearchText);
            }
            else
            {
                multiWordSearchText ??= GetMultiWordSearchText(searchTokens);
                score = GetScore(text, multiWordSearchText);
            }

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

    private string GetSingleWordSearchText(List<SearchToken> searchTokens)
    {
        return searchTokens.FirstOrDefault()?.Text ?? "";
    }

    private string GetMultiWordSearchText(List<SearchToken> searchTokens)
    {
        if (searchTokens.Count > 0 && searchTokens[0] is SpaceToken)
            return " ";

        var searchText = string.Join(" ", searchTokens.TakeWhile(x => x is WordToken).Select(x => x.Text));

        return searchText;
    }

    private int GetScore(string text, string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
            return 0;

        if (text.HasLength() == false)
            return 0;

        var filterAlgorithm = new FilterAlgorithm(searchText);

        var score = filterAlgorithm.GetScore(text);

        return score;
    }
}