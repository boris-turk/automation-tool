using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [Serializable]
    public class RequestDispatcher
    {
        private List<SearchEngineState> _states;
        private readonly List<Request> _rootRequests;
        private readonly ISearchEngine _searchEngine;

        public RequestDispatcher(List<Request> rootRequests, ISearchEngine searchEngine)
        {
            _states = new List<SearchEngineState>();

            _rootRequests = rootRequests;
            _searchEngine = searchEngine;
        }

        private List<Request> SearchItems => _searchEngine.Items;

        private SearchEngineState CurrentState => _states.Last();

        public void Reset()
        {
            _states.Clear();
        }

        public void Dispatch()
        {
            CreateInitialStateIfNecessary();

            if (!_states.Any())
                return;

            if (_searchEngine.ActionType == ActionType.MoveNext)
                OnMoveNext();
            else
                OnMovePrevious();

            SearchItems.Clear();

            var filter = new FilterAlgorithm(CurrentState.Text);

            var items = GetChildRequests(CurrentState.Request);
            items = filter.Filter(items);

            SearchItems.AddRange(items);
        }

        private bool ShouldMoveToNextItem()
        {
            return CurrentState.Text.EndsWith(" ");
        }

        private void OnMoveNext()
        {
            if (_searchEngine.SearchText.Length > 0)
                CurrentState.Text += _searchEngine.SearchText.Last();

            if (ShouldMoveToNextItem())
                _states.Add(new SearchEngineState(_searchEngine.SelectedItem));
        }

        private void OnMovePrevious()
        {
            if (CurrentState.Text.Length == 0 && _states.Count > 1)
                _states.RemoveAt(_states.Count - 1);

            if (CurrentState.Text.Length > 0)
                CurrentState.Text = CurrentState.Text.Remove(CurrentState.Text.Length - 1);
        }

        private void CreateInitialStateIfNecessary()
        {
            if (_states.Any())
                return;

            var rootRequest = GetRootRequest();

            if (rootRequest == null)
                return;

            _states.Add(new SearchEngineState(rootRequest));
        }

        private Request GetRootRequest()
        {
            var request = _rootRequests.FirstOrDefault(_ => GetChildRequests(_).Any());
            return request;
        }

        private IEnumerable<Request> GetChildRequests(Request request)
        {
            return request.ChildRequests(_searchEngine.Context);
        }

        private class SearchEngineState
        {
            public SearchEngineState(Request request)
            {
                Text = "";
                Request = request;
            }

            public Request Request { get; }

            public string Text { get; set; }
        }
    }
}
