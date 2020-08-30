using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public abstract class Command
    {
        private readonly List<SequentialRequest> _requests;

        protected Command()
        {
            _requests = new List<SequentialRequest>();
            Request = new CompositeRequest(_requests);
            AddRootCommandRequest();
        }

        public bool CanMoveNext { get; set; }

        public CompositeRequest Request { get; }

        protected abstract string CommandName { get; }

        private void AddRootCommandRequest()
        {
            AddRequest(new RootCommandRequest(CommandName));
        }

        protected void AddRequest(SequentialRequest rule)
        {
            _requests.Add(rule);
        }
    }
}