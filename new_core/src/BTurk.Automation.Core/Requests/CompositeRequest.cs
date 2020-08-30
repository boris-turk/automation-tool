using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public class CompositeRequest : Request
    {
        public CompositeRequest(IEnumerable<SequentialRequest> requests)
        {
            Requests = requests;
        }

        public IEnumerable<SequentialRequest> Requests { get; }
    }
}