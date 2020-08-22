namespace BTurk.Automation.Host.SearchEngine
{
    public interface ISearchHandlersCollection
    {
        void AddHandler(ISearchHandler handler);
        void RemoveHandler(ISearchHandler handler);
    }
}
