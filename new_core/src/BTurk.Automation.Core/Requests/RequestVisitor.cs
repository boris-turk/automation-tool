using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class RequestVisitor<TRequest> : IRequestVisitor<TRequest> where TRequest : Request
    {
        private readonly ISearchEngine _searchEngine;
        private readonly IRequestVisitor _requestVisitor;
        private readonly IChildRequestsProvider _childRequestsProvider;

        public RequestVisitor(ISearchEngine searchEngine, IRequestVisitor requestVisitor,
            IChildRequestsProvider childRequestsProvider)
        {
            _searchEngine = searchEngine;
            _childRequestsProvider = childRequestsProvider;
            _requestVisitor = requestVisitor;
        }

        private SearchStep CurrentStep => _searchEngine.Steps.Last();

        private List<Request> SearchItems => _searchEngine.Items;

        public void Visit(TRequest request)
        {
            var currentStep = CurrentStep;

            if (_searchEngine.ActionType == ActionType.Execute)
                OnExecute(request);

            if (_searchEngine.ActionType == ActionType.MoveNext)
                OnMoveNext(request);

            if (_searchEngine.ActionType == ActionType.MovePrevious)
                OnMovePrevious();

            if (currentStep == CurrentStep)
                LoadSearchItems();
        }

        private void OnExecute(TRequest request)
        {
            throw new System.NotImplementedException();
        }

        private void OnMoveNext(TRequest request)
        {
            var currentStep = CurrentStep;

            if (currentStep.Children.Count == 0)
                currentStep.Children.AddRange(GetChildren(request));

            var context = new VisitPredicateContext(currentStep.Text, _searchEngine.ActionType, _searchEngine.Context);
            var visitableChild = currentStep.Children.FirstOrDefault(_ => _.CanVisit(context));

            if (visitableChild != null)
                VisitChild(visitableChild);
        }

        private void VisitChild(Request child)
        {
            var step = new SearchStep(child);
            _searchEngine.Steps.Add(step);
            _requestVisitor.Visit(child);
        }

        private void OnMovePrevious()
        {
            if (CurrentStep.Text.Length == 0 && _searchEngine.Steps.Count > 2)
                _searchEngine.Steps.RemoveAt(_searchEngine.Steps.Count - 1);

            if (CurrentStep.Text.Length > 0)
                CurrentStep.Text = CurrentStep.Text.Remove(CurrentStep.Text.Length - 1);
        }

        private void LoadSearchItems()
        {
            SearchItems.Clear();

            if (CurrentStep.Request == null)
                return;

            var filter = new FilterAlgorithm(CurrentStep.Text);

            var items = filter.Filter(CurrentStep.Children);

            SearchItems.AddRange(items);
        }

        private IEnumerable<Request> GetChildren(TRequest request)
        {
            return _childRequestsProvider.LoadChildren(request);
        }
    }
}