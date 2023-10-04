using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.Core.Requests
{
    [DataContract]
    [DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
    public class Request : IRequest
    {
        public static readonly Request Null = new();

        public Request()
        {
        }

        public Request(string text)
        {
            Text = text;
        }

        [DataMember(Name = "Text")]
        public virtual string Text { get; set; }

        public ICommand Command { get; set; }

        public Predicate<DispatchPredicateContext> CanAcceptPredicate { get; set; }

        public override string ToString() => Text ?? "";

        protected virtual bool CanAccept(DispatchPredicateContext context)
        {
            if (context.ActionType == ActionType.MoveNext)
                return context.Text.Trim().Length > 0 && context.Text.EndsWith(" ");

            return context.ActionType == ActionType.Execute;
        }

        private string RequestTypeName => Extensions.GetDebuggerDisplayText(this);

        bool IRequest.CanAccept(DispatchPredicateContext context)
        {
            if (CanAcceptPredicate != null)
                return CanAcceptPredicate.Invoke(context);

            return CanAccept(context);
        }
    }
}