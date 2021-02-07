using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class ApplicationContextMenuRequestHandler : Request
    {
        private readonly IWindowContextProvider _windowContextProvider;

        public ApplicationContextMenuRequestHandler(IWindowContextProvider windowContextProvider)
            : base("")
        {
            _windowContextProvider = windowContextProvider;
        }

        public override IEnumerable<Request> ChildRequests()
        {
            if (_windowContextProvider.Context.IsEmpty)
                yield break;

            yield return new CommitRepositoryRequest();
        }
    }
}