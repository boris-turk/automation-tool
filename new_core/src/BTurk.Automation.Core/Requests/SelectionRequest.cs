using System;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<TRequest> : Request where TRequest : Request
    {
        public bool IsOptional { get; set; }

        public string OptionPrefix { get; set; }

        public Action<TRequest> Selected { get; set; }
    }
}