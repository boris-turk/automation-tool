using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.E3k
{
    public class FieldRequestHandler : NamedCommand
    {
        protected override string CommandName => "field";

        protected override IEnumerable<Request> CreateRequests()
        {
            return Enumerable.Empty<Request>();
        }
    }
}
