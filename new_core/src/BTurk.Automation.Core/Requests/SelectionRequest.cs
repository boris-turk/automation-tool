using System;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<TRequest> : Request, ISelectionRequest where TRequest : IRequest
    {
        public SelectionRequest()
        {
        }

        public SelectionRequest(string text)
        {
            Text = text;
        }

        public Action<TRequest> ChildExecuted { get; set; }

        public Action<TRequest> ChildSelected { get; set; }

        public Action<TRequest> ChildDeselected { get; set; }

        protected override bool CanVisit(VisitPredicateContext context)
        {
            return context.ActionType == ActionType.Execute || context.ActionType == ActionType.MoveNext;
        }
    }
}