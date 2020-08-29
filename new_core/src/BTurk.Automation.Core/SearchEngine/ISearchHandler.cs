namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchHandler<TRequest> where TRequest : Request
    {
        void Handle(TRequest request);
    }
}
