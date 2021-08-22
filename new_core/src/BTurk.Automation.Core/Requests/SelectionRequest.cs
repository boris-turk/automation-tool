using System;
using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<TRequest> : Request, ICollectionRequest<TRequest> where TRequest : IRequest
    {
        public Action<TRequest> ChildSelected { get; set; }

        public Action<TRequest> ChildDeselected { get; set; }

        protected override bool CanAccept(DispatchPredicateContext context)
        {
            return context.ActionType == ActionType.Execute || context.ActionType == ActionType.MoveNext;
        }

        public IEnumerable<TRequest> GetRequests(IRequestsProvider<TRequest> provider) => provider.GetRequests();
    }
}