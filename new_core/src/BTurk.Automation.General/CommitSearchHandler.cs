using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.General
{
    public class CommitSearchHandler : ISearchHandler
    {
        public SearchResultsCollection Handle(string text)
        {
            return SearchResultsCollection.Single("Commit");
        }
    }
}