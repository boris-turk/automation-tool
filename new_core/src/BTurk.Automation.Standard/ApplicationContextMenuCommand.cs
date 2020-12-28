using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class ApplicationContextMenuRequestHandler : ICommand
    {
        private int _count;
        private readonly ISearchEngine _searchEngine;

        public ApplicationContextMenuRequestHandler(ISearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }

        private void OnRepositorySelected(Repository repository)
        {
            repository.Commit();
        }

        public CompositeRequest Request => new CompositeRequest(CreateRequests());

        private IEnumerable<Request> CreateRequests()
        {
            _count += 1;

            if (_searchEngine.Context.IsEmpty)
                yield break;

            yield return new SelectionRequest<Repository>(OnRepositorySelected)
            {
                CanMoveNext = false
            };
        }
    }
}