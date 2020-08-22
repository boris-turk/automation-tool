namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchHandler
    {
        SearchResultsCollection Handle(SearchParameters parameters);
    }
}