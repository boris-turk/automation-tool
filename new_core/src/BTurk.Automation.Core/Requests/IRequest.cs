using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface IRequest
    {
        string Text { get; }
        bool CanAccept(DispatchPredicateContext context);
        IEnumerable<Request> ChildRequests(EnvironmentContext context);
    }
}