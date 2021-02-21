using System.Collections;
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
            {
                currentStep.Children.AddRange(GetChildren(request));
                VisitChildren(currentStep.Children);
            }
            else if (SearchItems.Count == 1)
            {
                VisitChild(SearchItems.Single());
            }
            else
            {
                if (_searchEngine.SearchText.Length > 0)
                    currentStep.Text += _searchEngine.SearchText.Last();
            }
        }

        private void VisitChildren(List<Request> children)
        {
            foreach (var child in children)
            {
                if (!(child is ISelectionRequest))
                    break;

                VisitChild(child);

                _requestVisitor.Visit(child);

                if (CurrentStep.Children.Any())
                    break;
            }
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