using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BTurk.Automation.Host.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class CommitSearchHandler : ISearchHandler
    {
        public SearchResultsCollection Handle(SearchParameters parameters)
        {
            var match = Regex.Match(parameters.Text, @"co\s(?<repo>.*)");

            if (parameters.ActionType == ActionType.Execution)
                Process.Start("mspaint.exe");

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
