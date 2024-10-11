using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.UnitTests
{
    public class FakeSearchEngine : ISearchEngineV2
    {
        private readonly List<SearchToken> _searchTokens = [];

        public FakeSearchEngine(IRequest rootMenuRequest)
        {
            RootMenuRequest = rootMenuRequest;
        }

        public void SetSearchTokens(params SearchToken[] searchTokens)
        {
            _searchTokens.AddRange(searchTokens ?? []);
        }

        public List<SearchResult> SearchResults { get; } = [];

        public EnvironmentContext Context { get; set; } = EnvironmentContext.Empty;

        public IRequestV2 RootMenuRequest { get; }

        List<SearchToken> ISearchEngineV2.SearchTokens
        {
            get => _searchTokens;
            set
            {
                _searchTokens.Clear();
                _searchTokens.AddRange(value ?? []);
            }
        }

        void ISearchEngineV2.SetSearchResults(List<SearchResult> resultsCollection)
        {
            SearchResults.AddRange(resultsCollection);
        }
    }
}