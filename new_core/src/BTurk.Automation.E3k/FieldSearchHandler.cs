using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k
{
    public class FieldSearchHandler : ISearchHandler
    {
        public SearchResultsCollection Handle(SearchParameters parameters)
        {
            return SearchResultsCollection.Single("Field");
        }
    }
}
