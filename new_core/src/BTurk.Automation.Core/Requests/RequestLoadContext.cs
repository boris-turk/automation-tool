using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class RequestLoadContext<TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Externally supplied requests (i.e., loaded by the associated provider).
        /// </summary>
        public List<TRequest> SuppliedRequests { get; }

        public EnvironmentContext EnvironmentContext { get; }

        public RequestLoadContext(EnvironmentContext environmentContext, List<TRequest> suppliedRequests)
        {
            EnvironmentContext = environmentContext;
            SuppliedRequests = suppliedRequests;
        }
    }
}