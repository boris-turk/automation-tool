using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.Requests
{
    public abstract class Command
    {
        protected Command()
        {
            Request = CreateCompositeRequest();
        }

        private CompositeRequest CreateCompositeRequest()
        {
            var requests = CreateRequests().Prepend(new CommandRequest(CommandName));
            var request = new CompositeRequest(requests);
            return request;
        }

        public bool CanMoveNext { get; set; }

        public CompositeRequest Request { get; }

        protected abstract string CommandName { get; }

        protected abstract IEnumerable<Request> CreateRequests();
    }
}