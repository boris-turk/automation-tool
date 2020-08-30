using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class RepositoryRequestHandler : IRequestHandler<SelectionRequest<Repository>>
    {
        private readonly ISearchEngine _searchEngine;
        private readonly ISearchItemsProvider _itemsProvider;

        public RepositoryRequestHandler(ISearchItemsProvider itemsProvider, ISearchEngine searchEngine)
        {
            _itemsProvider = itemsProvider;
            _searchEngine = searchEngine;
        }

        public void Handle(SelectionRequest<Repository> request)
        {
            _itemsProvider.Items.Clear();
            _searchEngine.AddItems(GetRepositories());
            _searchEngine.FilterText = request.FilterProvider?.Invoke(_searchEngine.SearchText);
        }

        public IEnumerable<SearchItem> GetRepositories()
        {
            yield return new Repository("trunk");
            yield return new Repository("r8");
            yield return new Repository("r7");
            yield return new Repository("r6");
        }
    }
}