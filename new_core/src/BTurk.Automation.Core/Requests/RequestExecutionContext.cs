using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable NotResolvedInText

namespace BTurk.Automation.Core.Requests
{
    public class RequestExecutionContext
    {
        public RequestExecutionContext(List<Request> currentRequests)
        {
            if (currentRequests == null)
                throw new ArgumentNullException(nameof(currentRequests));

            if (currentRequests.Count == 0)
                throw new ArgumentException(nameof(currentRequests), "No requests provided");

            Request = currentRequests.Last();
            ParentRequests = currentRequests.Take(currentRequests.Count - 1).ToList();
        }

        public Request Request { get; }

        public List<Request> ParentRequests { get; }
    }

    public class RequestExecutionContext<TRequest> where TRequest : Request
    {
        public TRequest Request { get; private set; }

        public List<Request> ParentRequests { get; private set; }

        public static implicit operator RequestExecutionContext<TRequest>(RequestExecutionContext context)
        {
            return new RequestExecutionContext<TRequest>
            {
                Request = (TRequest) context.Request,
                ParentRequests = context.ParentRequests
            };
        }
    }
}