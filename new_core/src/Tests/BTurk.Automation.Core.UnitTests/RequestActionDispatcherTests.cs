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
    public void Dispatch_WithEmptyRootRequestAndEmptySearch_LoadsNoSearchResults()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(new FakeRequest("Empty"));
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        searchEngine.SearchResults.Should().BeEmpty();
    }

    [Fact]
    public void Dispatch_WithEmptySearchAndParentGroups_SearchesInEachLowestGroup()
    {
        // Arrange
        var searchEngine = new FakeSearchEngine(
            new FakeRequest("RootMenu").WithChildren(
                new FakeRequest("MainMenu").WithChildren(
                    new FakeRequest("SubMenu").WithChildren(
                        new FakeRequest("CommitMenu", "commit")
                    ),
                    new FakeRequest("Solution", "solution")
                ),
                new FakeRequest("Gvim", "gvim")
            )
        );
        var sut = GetRequestActionDispatcher(searchEngine);

        // Act
        sut.Dispatch(ActionType.Search);

        // Assert
        AssertSearchResults(searchEngine, [
            ["RootMenu", "MainMenu", "SubMenu", "CommitMenu"],
            ["RootMenu", "MainMenu", "Solution"],
            ["RootMenu", "Gvim"],
        ]);
    }

    private RequestActionDispatcherV2 GetRequestActionDispatcher(ISearchEngineV2 searchEngine,
        IChildRequestsProviderV2 childRequestsProvider = null)
    {
        return new RequestActionDispatcherV2(searchEngine, childRequestsProvider ?? new FakeChildRequestsProvider());
    }

    private void AssertSearchResults(FakeSearchEngine searchEngine, List<List<string>> expectedFriendlyNamesCollection)
    {
        var searchResults = searchEngine.SearchResults;

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
                var friendlyName = (items[j].Request as FakeRequest)?.FriendlyName;
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