using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [DataContract]
    [DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
    public class Request : IRequest
    {
        public event Action Loaded;
        public event Action Unloaded;

        public Request()
        {
        }

        public Request(string text)
        {
            Text = text;
        }

        [DataMember(Name = "Text")]
        public string Text { get; set; }

        public Predicate<VisitPredicateContext> CanVisitPredicate { get; set; }

        protected virtual IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            return Enumerable.Empty<Request>();
        }

        public override string ToString() => Text ?? "";

        protected virtual bool CanVisit(VisitPredicateContext context)
        {
            if (context.ActionType == ActionType.MoveNext)
                return context.Text.Trim().Length > 0 && context.Text.EndsWith(" ");

            return context.ActionType == ActionType.Execute;
        }

        private string RequestTypeName => Extensions.GetDebuggerDisplayText(this);

        void IRequest.Load() => Loaded?.Invoke();

        void IRequest.Unload() => Unloaded?.Invoke();

        bool IRequest.CanVisit(VisitPredicateContext context)
        {
            if (CanVisitPredicate != null)
                return CanVisitPredicate.Invoke(context);

            return CanVisit(context);
        }

        IEnumerable<Request> IRequest.ChildRequests(EnvironmentContext context) => ChildRequests(context);
    }
}