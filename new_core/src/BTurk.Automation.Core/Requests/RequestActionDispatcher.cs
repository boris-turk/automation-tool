﻿using System;
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

        var rootSearchResult = new SearchResult().Append(rootRequest);
        var searchScope = new SearchScope(searchTokens);
        var searchResults = CollectSearchResults(rootSearchResult, searchScope).ToList();

        SearchEngine.SetSearchResults(searchResults);
    }

    private IEnumerable<SearchResult> CollectSearchResults(SearchResult result, SearchScope searchScope)
    {
        var searchTokens = searchScope.SearchTokens;

        var request = result.Items.Last().Request;

        if (searchScope.ShouldSkipSearchResult(request, result))
        {
            yield break;
        }

        if (searchScope.ShouldSkipChildren(searchTokens, request))
        {
            yield return result;
            yield break;
        }

        var childrenCollection = GetChildren(request, searchTokens)
            .OrderByDescending(c => c.Score)
            .ToList();

        foreach (var child in childrenCollection)
        {
            var childResult = result.Append(child.Request);

            var childSearchScope = searchScope.GetChildrenSearchScope(child);

            if (childSearchScope.ShouldSkipChild(child))
                continue;

            foreach (var grandChildResult in CollectSearchResults(childResult, childSearchScope))
            {
                yield return grandChildResult;
            }
        }
    }

    private IEnumerable<RequestMatch> GetChildren(IRequest request, List<SearchToken> searchTokens)
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
                yield return new(score, child);
                continue;
            }

            if (!text.HasLength())
            {
                yield return new(score: 1, child);
                continue;
            }

            if (!searchTokens.Any() || child.Configuration.ScanChildrenIfUnmatched)
            {
                yield return new(score: 1, child);
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

    private class SearchScope
    {
        private readonly List<SearchToken> _previousSearchTokens = new();

        public SearchScope(List<SearchToken> searchTokens)
        {
            SearchTokens = searchTokens;
        }

        public List<SearchToken> SearchTokens { get; }

        public bool ShouldSkipSearchResult(IRequest request, SearchResult result)
        {
            if (request.Configuration.CanHaveChildren)
                return false;

            return result.Items.Count == 1;
        }

        public bool ShouldSkipChildren(List<SearchToken> searchTokens, IRequest request)
        {
            if (request.Configuration.Text.HasLength() && !searchTokens.Any())
                return true;

            if (request.Configuration.CanHaveChildren)
                return false;

            if (!searchTokens.Any())
                return true;

            return false;
        }

        public SearchScope GetChildrenSearchScope(RequestMatch match)
        {
            var childSearchTokens = GetChildSearchTokens(match);
            var scope = new SearchScope(childSearchTokens);
            scope._previousSearchTokens.AddRange(SearchTokens);
            return scope;
        }

        private List<SearchToken> GetChildSearchTokens(RequestMatch match)
        {
            const int sufficientMatch = 1;
            const int optimalMatch = 1000;
            const int perfectMatch = 5000;

            var request = match.Request;
            var text = request.Configuration.Text;

            if (!text.HasLength())
                return SearchTokens.ToList();

            var skipCount = match.Score switch
            {
                sufficientMatch when !request.Configuration.ScanChildrenIfUnmatched => 1,
                sufficientMatch when SearchTokens.All(x => x is SpaceToken) => 1,
                sufficientMatch => 0,
                >= perfectMatch when request.Configuration.CanHaveChildren => 1,
                < optimalMatch => SearchTokens.TakeWhile(x => x is WordToken).Count(),
                >= optimalMatch => SearchTokens.TakeWhile(x => x is WordToken).Count()
            };

            return SearchTokens.Skip(skipCount).ToList();
        }

        public bool ShouldSkipChild(RequestMatch match)
        {
            return _previousSearchTokens.Count - SearchTokens.Count > 1 && match.Request.Configuration.CanHaveChildren;
        }
    }

    private class RequestMatch
    {
        public RequestMatch(int score, IRequest request)
        {
            Score = score;
            Request = request;
        }

        public int Score { get; }

        public IRequest Request { get; }
    }
}