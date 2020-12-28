using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.Requests
{
    public abstract class NamedCommand : ICommand
    {
        private CompositeRequest CreateCompositeRequest()
        {
            var requests = CreateRequests().Prepend(new CommandRequest(CommandName));
            return new CompositeRequest(requests);
        }

        public CompositeRequest Request => CreateCompositeRequest();

        protected abstract string CommandName { get; }

        protected abstract IEnumerable<Request> CreateRequests();
    }
}