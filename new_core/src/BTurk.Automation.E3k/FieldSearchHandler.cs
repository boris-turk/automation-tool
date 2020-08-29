using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k
{
    public class FieldSearchHandler : RootSearchHandler
    {
        public FieldSearchHandler(ISearchHandler<CompositeRequest> searchHandler) : base(searchHandler)
        {
            AddRequest(new RootCommandRequest("field"));
        }
    }
}
