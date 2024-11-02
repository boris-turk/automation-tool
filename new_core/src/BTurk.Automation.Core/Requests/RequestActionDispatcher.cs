using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestActionDispatcher : IRequestActionDispatcher
{
    public RequestActionDispatcher(ISearchEngine searchEngine, ICommandProcessor commandProcessor,
        IChildRequestsProvider childRequestsProvider)
    {
        SearchEngine = searchEngine;
        CommandProcessor = commandProcessor;
        ChildRequestsProvider = childRequestsProvider;
    }

    private ISearchEngine SearchEngine { [DebuggerStepThrough] get; }

    private ICommandProcessor CommandProcessor { [DebuggerStepThrough] get; }

    private IChildRequestsProvider ChildRequestsProvider { get; }

    public void Dispatch(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Execute:
                Execute();
                break;
            case ActionType.Search:
                Search();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        }
    }

    private void Execute()
    {
        var items = SearchEngine.SelectedSearchResult?.Items;

        if (items == null)
            return;

        var executed = false;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            var childRequest = items.ElementAtOrDefault(i + 1)?.Request;

            var configuration = items[i].Request.Configuration;
            executed = configuration.ExecuteCommand(CommandProcessor, childRequest);

            if (executed)
                break;
        }

        if (executed)
            SearchEngine.Hide();
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

            if (ShouldSkipSearchResult(request, result))
            {
                resultsCollection.Remove(result);
                continue;
            }

            if (ShouldSkipChildren(searchTokens, request))
            {
                continue;
            }

            var childrenCollection = GetChildren(request, searchTokens).ToList();

            var insertIndex = resultsCollection.IndexOf(result);
            resultsCollection.Remove(result);

            foreach (var child in childrenCollection.OrderByDescending(c => c.Score))
            {
                var childResultsCollection = new List<SearchResult> { result.Append(child.Request) };

                var childSearchTokens = GetChildSearchTokens(child, searchTokens);

                if (searchTokens.Count - childSearchTokens.Count > 1 && child.Request.Configuration.CanHaveChildren)
                    continue;

                CollectSearchResults(childResultsCollection, childSearchTokens);

                resultsCollection.InsertRange(insertIndex, childResultsCollection);

                insertIndex += childResultsCollection.Count;
            }
        }
    }

    private static bool ShouldSkipSearchResult(IRequest request, SearchResult result)
    {
        if (request.Configuration.CanHaveChildren)
            return false;

        return result.Items.Count == 1;
    }

    private static bool ShouldSkipChildren(List<SearchToken> searchTokens, IRequest request)
    {
        if (request.Configuration.Text.HasLength() && !searchTokens.Any())
            return true;

        if (request.Configuration.CanHaveChildren)
            return false;

        if (!searchTokens.Any())
            return true;

        return false;
    }

    private List<SearchToken> GetChildSearchTokens((int Score, IRequest Request) child, List<SearchToken> searchTokens)
    {
        const int sufficientMatch = 1;
        const int optimalMatch = 1000;
        const int perfectMatch = 5000;

        var request = child.Request;
        var text = request.Configuration.Text;

        if (!text.HasLength())
            return searchTokens.ToList();

        var skipCount = child.Score switch
        {
            sufficientMatch when !request.Configuration.ScanChildrenIfUnmatched => 1,
            sufficientMatch when searchTokens.All(x => x is SpaceToken) => 1,
            sufficientMatch => 0,
            >= perfectMatch when request.Configuration.CanHaveChildren => 1,
            < optimalMatch => searchTokens.TakeWhile(x => x is WordToken).Count(),
            >= optimalMatch => searchTokens.TakeWhile(x => x is WordToken).Count()
        };

        return searchTokens.Skip(skipCount).ToList();
    }

    private IEnumerable<(int Score, IRequest Request)> GetChildren(IRequest request, List<SearchToken> searchTokens)
    {
        string singleWordSearchText = null;
        string multiWordSearchText = null;

        foreach (var child in request.Configuration.GetChildren(ChildRequestsProvider))
        {
            if (!child.Configuration.CanProcess(SearchEngine.Context))
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

            if (!text.HasLength())
            {
                yield return (Score: 1, child);
                continue;
            }

            if (!searchTokens.Any() || child.Configuration.ScanChildrenIfUnmatched)
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

        if (!text.HasLength())
            return 0;

        var filterAlgorithm = new FilterAlgorithm(searchText);

        var score = filterAlgorithm.GetScore(text);

        return score;
    }
}