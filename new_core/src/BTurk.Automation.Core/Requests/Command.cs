using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public abstract class Command
    {
        private readonly List<Request> _requests;

        protected Command()
        {
            _requests = new List<Request>();
            Request = new CompositeRequest(_requests);
            AddCommandRequest();
        }

        public bool CanMoveNext { get; set; }

        public CompositeRequest Request { get; }

        protected abstract string CommandName { get; }

        private void AddCommandRequest()
        {
            AddRequest(new CommandRequest(CommandName));
        }

        protected void AddRequest(Request rule)
        {
            _requests.Add(rule);
        }
    }
}