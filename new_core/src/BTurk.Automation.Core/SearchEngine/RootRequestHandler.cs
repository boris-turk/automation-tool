using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    public abstract class RootRequestHandler : IRequestHandler<Request>
    {
        private readonly IRequestHandler<CompositeRequest> _requestHandler;

        protected RootRequestHandler(IRequestHandler<CompositeRequest> requestHandler)
        {
            _requestHandler = requestHandler;
            Requests = new List<SequentialRequest>();
            AddRootCommandRequest();
        }

        protected abstract string CommandName { get; }

        public List<SequentialRequest> Requests { get; }

        private void AddRootCommandRequest()
        {
            AddRequest(new RootCommandRequest(CommandName));
        }

        protected void AddRequest(SequentialRequest rule)
        {
            Requests.Add(rule);
        }

        public void Handle(Request request)
        {
            request.Handled = false;

            if (!Requests.Any())
                return;

            var compositeRequest = new CompositeRequest(Requests);

            _requestHandler.Handle(compositeRequest);

            request.Handled = Requests.Any(_ => _.Handled);
        }
    }
}