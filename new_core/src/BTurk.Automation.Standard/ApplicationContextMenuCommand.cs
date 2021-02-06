using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class ApplicationContextMenuRequestHandler : ICommand
    {
        private readonly IWindowContextProvider _windowContextProvider;

        public ApplicationContextMenuRequestHandler(IWindowContextProvider windowContextProvider)
        {
            _windowContextProvider = windowContextProvider;
        }

        private void OnRepositorySelected(Repository repository)
        {
            repository.Commit();
        }

        public CompositeRequest Request => new CompositeRequest(CreateRequests());

        private IEnumerable<Request> CreateRequests()
        {
            if (_windowContextProvider.Context.IsEmpty)
                yield break;

            yield return new SelectionRequest<Repository>(OnRepositorySelected)
            {
                CanMoveNext = false
            };
        }
    }
}