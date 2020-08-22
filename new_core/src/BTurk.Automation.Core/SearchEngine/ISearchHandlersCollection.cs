namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchHandlersCollection
    {
        void AddHandler(ISearchHandler handler);
        void RemoveHandler(ISearchHandler handler);
    }
}
