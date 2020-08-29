using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class CommitRequestHandler : RootRequestHandler
    {
        public CommitRequestHandler(IRequestHandler<CompositeRequest> requestHandler) :
            base(requestHandler)
        {
            AddRequest(new RootCommandRequest("commit"));
        }
    }
}
