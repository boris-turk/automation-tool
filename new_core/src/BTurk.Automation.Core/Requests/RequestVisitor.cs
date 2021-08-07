using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class RequestVisitor<TRequest> : IRequestVisitor<TRequest> where TRequest : IRequest
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

        private List<IRequest> SearchItems => _searchEngine.Items;

        public void Visit(TRequest request, ActionType actionType)
        {
            if (actionType == ActionType.Execute)
                OnExecute(request);

            if (actionType == ActionType.MoveNext)
                OnMoveNext(request);

            if (actionType == ActionType.MovePrevious)
                OnMovePrevious();

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

            if (!currentStep.Children.Any())
            {
                RemoveLastStep();
                return;
            }

            var context = new VisitPredicateContext(currentStep.Text, ActionType.MoveNext, _searchEngine.Context);
            var visitableChild = currentStep.Children.FirstOrDefault(_ => _.CanVisit(context));

            if (visitableChild != null)
                VisitChild(visitableChild, ActionType.MoveNext);
        }

        private void VisitChild(IRequest child, ActionType actionType)
        {
            var step = new SearchStep(child);
            _searchEngine.Steps.Add(step);
            _requestVisitor.Visit(child, actionType);
        }

        private void OnMovePrevious()
        {
            if (CurrentStep.Text.Length == 0 && _searchEngine.Steps.Count > 2)
            {
                RemoveLastStep();
                VisitCurrentRequest(ActionType.MovePrevious);
            }
            else if (CurrentStep.Text.Length > 0)
            {
                CurrentStep.Text = CurrentStep.Text.Remove(CurrentStep.Text.Length - 1);
                VisitCurrentRequest(ActionType.MoveNext);
            }
        }

        private void VisitCurrentRequest(ActionType actionType)
        {
            _requestVisitor.Visit(CurrentStep.Request, actionType);
        }

        private void RemoveLastStep()
        {
            var lastStep = _searchEngine.Steps.Last();
            lastStep.Request.Unload();
            _searchEngine.Steps.Remove(lastStep);
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

        private IEnumerable<IRequest> GetChildren(TRequest request)
        {
            return _childRequestsProvider.LoadChildren(request);
        }
    }
}