using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestProcessor
    {
        IEnumerable<Request> LoadChildren(Request request);
        void Execute(RequestExecutionContext context);
    }
}