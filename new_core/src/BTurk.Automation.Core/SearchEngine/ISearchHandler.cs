namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchHandler
    {
        SearchResult Handle(string text);
    }
}