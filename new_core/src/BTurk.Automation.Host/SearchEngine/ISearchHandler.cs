namespace BTurk.Automation.Host.SearchEngine
{
    public interface ISearchHandler
    {
        SearchResultsCollection Handle(SearchParameters parameters);
    }
}
