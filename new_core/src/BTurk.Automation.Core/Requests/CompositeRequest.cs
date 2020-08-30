using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public class CompositeRequest : IRequest
    {
        public CompositeRequest(IEnumerable<Request> requests)
        {
            Requests = requests;
        }

        public IEnumerable<Request> Requests { get; }
    }
}