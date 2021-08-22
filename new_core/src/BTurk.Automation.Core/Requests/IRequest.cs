using System.Collections.Generic;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface IRequest
    {
        ICommand Command { get; }
        string Text { get; }
        bool CanAccept(DispatchPredicateContext context);
        IEnumerable<Request> ChildRequests(EnvironmentContext context);
    }
}