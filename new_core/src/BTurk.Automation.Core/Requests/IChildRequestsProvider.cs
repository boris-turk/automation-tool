using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public interface IChildRequestsProvider
    {
        IEnumerable<Request> LoadChildren(Request request);
    }
}