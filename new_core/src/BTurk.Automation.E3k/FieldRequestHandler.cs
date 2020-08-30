using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k
{
    public class FieldRequestHandler : RootRequestHandler
    {
        public FieldRequestHandler(IRequestHandler<CompositeRequest> requestHandler) : base(requestHandler)
        {
        }

        protected override string CommandName => "field";
    }
}
