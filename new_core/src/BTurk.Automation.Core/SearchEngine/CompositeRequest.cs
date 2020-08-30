using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine
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