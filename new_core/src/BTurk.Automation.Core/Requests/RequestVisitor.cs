using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class RequestVisitor<TRequest> : IRequestVisitor<TRequest> where TRequest : Request
    {
        private readonly ISearchEngine _searchEngine;
        private readonly IRequestVisitor _requestVisitor;
        private readonly IRequestsProvider<TRequest> _requestsProvider;

        public RequestVisitor(ISearchEngine searchEngine, IRequestVisitor requestVisitor,
            IRequestsProvider<TRequest> requestsProvider)
        {
            _searchEngine = searchEngine;
            _requestsProvider = requestsProvider;
            _requestVisitor = requestVisitor;
        }

        private SearchStep CurrentStep { get; set; }

        private List<Request> SearchItems => _searchEngine.Items;

        private bool IsMovingNext => _searchEngine.ActionType == ActionType.MoveNext;

        public void Visit(TRequest request)
        {
            CurrentStep = _searchEngine.Steps.Last();

            SearchItems.Clear();

            if (IsMovingNext)
                OnMoveNext(request);
        }

        private void LoadSearchItems()
        {
            if (CurrentStep.Request == null)
                return;

            var filter = new FilterAlgorithm(CurrentStep.Text);

            var items = filter.Filter(CurrentStep.Children);

            SearchItems.AddRange(items);
        }

        private void OnMoveNext(TRequest request)
        {
            CurrentStep.Children.AddRange(GetChildren(request));

            foreach (var child in CurrentStep.Children)
            {
                if (!(child is ISelectionRequest))
                {
                    LoadSearchItems();
                    return;
                }

                AddStep(child);

                _requestVisitor.Visit(child);

                if (_searchEngine.Steps.Last().Children.Any())
                    break;
            }
        }

        private void AddStep(Request nextRequest)
        {
            var step = new SearchStep(nextRequest);
            _searchEngine.Steps.Add(step);
        }

        private IEnumerable<Request> GetChildren(TRequest request)
        {
            var children = request.ChildRequests(_searchEngine.Context).ToList();

            if (children.Any())
                return children;

            return _requestsProvider.Load();
        }
    }
}