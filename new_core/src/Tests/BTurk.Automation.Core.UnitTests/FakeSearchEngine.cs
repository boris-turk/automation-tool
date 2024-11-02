using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.UnitTests
{
    public class FakeSearchEngine : ISearchEngine
    {
        private readonly List<SearchToken> _searchTokens = [];

        public FakeSearchEngine(IRequest rootMenuRequest)
        {
            RootMenuRequest = rootMenuRequest;
        }

        public int SelectedSearchResultIndex { get; set; }

        public void SetSearchTokens(string searchText)
        {
            var searchTokens = SearchToken.GetSearchTokens(searchText);
            _searchTokens.AddRange(searchTokens ?? []);
        }

        public List<SearchResult> SearchResults { get; } = [];

        public bool Hidden { get; private set; }

        public EnvironmentContext Context { get; set; } = EnvironmentContext.Empty;

        public IRequest RootMenuRequest { get; }

        void ISearchEngine.SetSearchResults(List<SearchResult> resultsCollection)
        {
            SearchResults.AddRange(resultsCollection);
        }

        void ISearchEngine.Hide()
        {
            Hidden = true;
        }

        List<SearchToken> ISearchEngine.SearchTokens
        {
            get => _searchTokens;
            set
            {
                _searchTokens.Clear();
                _searchTokens.AddRange(value ?? []);
            }
        }

        SearchResult ISearchEngine.SelectedSearchResult => SearchResults.ElementAtOrDefault(SelectedSearchResultIndex);
    }
}