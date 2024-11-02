using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.UnitTests
{
    public class FakeSearchEngine : ISearchEngineV2
    {
        private readonly List<SearchToken> _searchTokens = [];

        public FakeSearchEngine(IRequestV2 rootMenuRequest)
        {
            RootMenuRequest = rootMenuRequest;
        }

        public int SelectedSearchResultIndex { get; set; }

        public void SetSearchTokens(params SearchToken[] searchTokens)
        {
            _searchTokens.AddRange(searchTokens ?? []);
        }

        public List<SearchResult> SearchResults { get; } = [];

        public bool Hidden { get; private set; }

        public EnvironmentContext Context { get; set; } = EnvironmentContext.Empty;

        public IRequestV2 RootMenuRequest { get; }

        void ISearchEngineV2.SetSearchResults(List<SearchResult> resultsCollection)
        {
            SearchResults.AddRange(resultsCollection);
        }

        void ISearchEngineV2.Hide()
        {
            Hidden = true;
        }

        List<SearchToken> ISearchEngineV2.SearchTokens
        {
            get => _searchTokens;
            set
            {
                _searchTokens.Clear();
                _searchTokens.AddRange(value ?? []);
            }
        }

        SearchResult ISearchEngineV2.SelectedSearchResult => SearchResults.ElementAtOrDefault(SelectedSearchResultIndex);
    }
}