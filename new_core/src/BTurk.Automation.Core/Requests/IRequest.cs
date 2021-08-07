using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface IRequest
    {
        string Text { get; }
        void Load();
        void Unload();
        bool CanVisit(VisitPredicateContext context);
        IEnumerable<Request> ChildRequests(EnvironmentContext context);
    }
}