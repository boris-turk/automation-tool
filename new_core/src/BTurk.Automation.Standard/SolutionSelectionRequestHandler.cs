using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class SolutionSelectionRequestHandler : IRequestHandler<SelectionRequest<Solution>>
    {
        private readonly ISearchEngine _searchEngine;
        private readonly IResourceProvider _resourceProvider;

        public SolutionSelectionRequestHandler(ISearchEngine searchEngine, IResourceProvider resourceProvider)
        {
            _searchEngine = searchEngine;
            _resourceProvider = resourceProvider;
        }

        public void Handle(SelectionRequest<Solution> request)
        {
            _searchEngine.AddItems(GetRepositories());
            _searchEngine.FilterText = request.FilterTextProvider?.Invoke(_searchEngine.SearchText);
        }

        public IEnumerable<Solution> GetRepositories()
        {
            return _resourceProvider.Load<List<Solution>>("solutions");
        }
    }
}
