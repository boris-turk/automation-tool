using System;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<TRequest> : CollectionRequest<TRequest> where TRequest : IRequest
    {
        public Action<TRequest> ChildSelected { get; set; }

        public Action<TRequest> ChildDeselected { get; set; }

        protected override bool CanAccept(DispatchPredicateContext context)
        {
            return context.ActionType is ActionType.Execute or ActionType.MoveNext;
        }
    }
}