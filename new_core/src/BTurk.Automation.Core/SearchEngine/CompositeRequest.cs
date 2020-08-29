using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
{
    public class CompositeRequest : Request
    {
        public CompositeRequest(IEnumerable<Request> requests)
        {
            Requests = requests;
        }

        public IEnumerable<Request> Requests { get; }

        public Request HandledRequest => Requests.FirstOrDefault(_ => _.Handled);
    }
}