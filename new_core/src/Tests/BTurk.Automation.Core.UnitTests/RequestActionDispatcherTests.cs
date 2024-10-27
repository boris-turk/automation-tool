using System;
using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using FluentAssertions;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace BTurk.Automation.Core.UnitTests;

public class RequestActionDispatcherTests
{
    [Fact]
    public void Dispatch_WithEmptyRootRequestAndEmptySearch_LoadsRootRequest()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(new FakeRequest(name: "RootMenu"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        searchEngine.SearchResults.Should().BeEmpty();
    }

    [Fact]
    public void Dispatch_WithMatchingSearchTokens_LoadsOnlyMatchingResults()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "Solution", text: "solution"),
                new FakeRequest(name: "Gvim", text: "gvim")
            )
        );
        searchEngine.SetSearchTokens(new WordToken("so"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "Solution"]
        ]);
    }

    [Fact]
    public void Dispatch_WithNonMatchingSearchTokens_LoadsNoResults()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "Solution", text: "solution"),
                new FakeRequest(name: "Gvim", text: "gvim")
            )
        );
        searchEngine.SetSearchTokens(new WordToken("co"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, []);
    }

    [Fact]
    public void Dispatch_WithWordTokenFollowedBySpaceToken_LoadsChildrenOfMatchingMenu()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "Solution", text: "solution").WithChildren(
                    new FakeRequest(name: "s1", text: "e3k_trunk"),
                    new FakeRequest(name: "s2", "e3k_r11")
                ),
                new FakeRequest(name: "Gvim", text: "gvim")
            )
        );
        searchEngine.SetSearchTokens(new WordToken("so"), new SpaceToken());
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "Solution", "s1"],
            ["RootMenu", "Solution", "s2"]
        ]);
    }

    [Fact]
    public void Dispatch_EmptySearch_CollectsNonEmptyTextMenusAndSkipsTheirChildren()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "Menu", text: "menu").WithChildren(
                    new FakeRequest(name: "Item1", "item1"),
                    new FakeRequest(name: "Item2", "item2")
                    )
            )
        );
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "Menu"]
        ]);
    }

    [Fact]
    public void Dispatch_EmptySearch_CollectsNonEmptyTextMenusAndContinuesScanningOtherBranches()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "MainMenu").WithChildren(
                    new FakeRequest(name: "SubMenu").WithChildren(
                        new FakeRequest(name: "CommitMenu", text: "commit")
                    ),
                    new FakeRequest(name: "Solution", text: "solution")
                ),
                new FakeRequest(name: "Gvim", text: "gvim")
            )
        );
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "MainMenu", "SubMenu", "CommitMenu"],
            ["RootMenu", "MainMenu", "Solution"],
            ["RootMenu", "Gvim"],
        ]);
    }

    [Fact]
    public void Dispatch_ChildMenuUnmatchedAndScanChildrenIfUnmatchedEnabled_ScansGrandchildren()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "ChildMenu", text: "Text").ScanChildrenIfUnmatched().WithChildren(
                    new FakeRequest(name: "child1", text: "first child"),
                    new FakeRequest(name: "child2", text: "second child")
                )
            )
        );
        searchEngine.SetSearchTokens(new WordToken("fi"), new WordToken("ch"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "ChildMenu", "child1"]
        ]);
    }

    [Fact]
    public void Dispatch_RootWithoutMatchAndNonEmptyTextAndScanChildrenIfUnmatchedEnabled_LoadsMatchingChild()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu", text: "Menu").ScanChildrenIfUnmatched().WithChildren(
                new FakeRequest(name: "ChildMenu", text: "abc")
            )
        );
        searchEngine.SetSearchTokens(new WordToken("ab"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "ChildMenu"]
        ]);
    }

    [Fact]
    public void Dispatch_MenuWithNonEmptyTextAndMatchingFirstWord_UsesRemainingWordsToMatchChild()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest(name: "RootMenu").WithChildren(
                new FakeRequest(name: "Url", text: "url").WithChildren(
                    new FakeRequest(name: "url1", text: "abc def ghi"),
                    new FakeRequest(name: "url2", text: "abc jkl mno")
                )
            )
        );
        searchEngine.SetSearchTokens(new WordToken("u"), new WordToken("ab"), new WordToken("ef"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine.SearchResults, [
            ["RootMenu", "Url", "url1"]
        ]);
    }

    private RequestActionDispatcherV2 GetRequestActionDispatcher(ISearchEngineV2 searchEngine,
        IChildRequestsProviderV2 childRequestsProvider = null)
    {
        return new RequestActionDispatcherV2(searchEngine, childRequestsProvider ?? new FakeChildRequestsProvider());
    }

    private void AssertSearchResults(List<SearchResult> searchResults, List<List<string>> expectedFriendlyNamesCollection)
    {
        if (searchResults.Count != expectedFriendlyNamesCollection.Count)
        {
            throw new InvalidOperationException(
                $"Result collection has {searchResults.Count} element(s) " +
                $"instead of expected {expectedFriendlyNamesCollection.Count}");
        }

        for (int i = 0; i < searchResults.Count; i++)
        {
            var items = searchResults[i].Items;

            if (items.Count != expectedFriendlyNamesCollection[i].Count)
            {
                throw new InvalidOperationException(
                    $"Result with index {i} has {items.Count} item(s) " +
                    $"instead of expected {expectedFriendlyNamesCollection[i].Count}");
            }

            for (int j = 0; j < items.Count; j++)
            {
                var friendlyName = (items[j].Request as FakeRequest)?.Name;
                var expectedFriendlyName = expectedFriendlyNamesCollection[i][j];

                if (friendlyName != expectedFriendlyName)
                {
                    throw new InvalidOperationException(
                        $"Result with index {i} has item with index {j} and friendly name {friendlyName} " +
                        $"instead of expected friendly name {expectedFriendlyName}");
                }
            }
        }
    }
}
