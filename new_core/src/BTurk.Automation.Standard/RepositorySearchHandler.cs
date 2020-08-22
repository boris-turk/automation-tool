﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class RepositorySearchHandler : ISearchHandler
    {
        public SearchResultsCollection Handle(SearchParameters parameters)
        {
            var match = Regex.Match(parameters.Text, @"commit\s(?<repo>.*)");

            if (match.Success)
                return new SearchResultsCollection(GetRepositories());

            return SearchResultsCollection.Single("Commit");
        }

        private IEnumerable<SearchItem> GetRepositories()
        {
            yield return new SearchItem {Text = "trunk"};
            yield return new SearchItem {Text = "r5"};
            yield return new SearchItem {Text = "r6"};
            yield return new SearchItem {Text = "r7"};
            yield return new SearchItem {Text = "r8"};
        }
    }
}