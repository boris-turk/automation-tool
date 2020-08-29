using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class CommitSearchHandler : RootSearchHandler
    {
        public CommitSearchHandler(ISearchHandler<CompositeRequest> searchHandler) :
            base(searchHandler)
        {
            AddRequest(new RootCommandRequest("commit"));
        }
    }
}
