using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class RequestActionDispatcher<TRequest> : IRequestActionDispatcher<TRequest> where TRequest : class, IRequest
    {
        private readonly IRequestVisitor _visitor;
        private readonly ISearchEngine _searchEngine;
        private readonly IRequestActionDispatcher _dispatcher;
        private readonly IChildRequestsProvider _childRequestsProvider;

        public RequestActionDispatcher(IRequestVisitor visitor, ISearchEngine searchEngine,
            IRequestActionDispatcher dispatcher, IChildRequestsProvider childRequestsProvider)
        {
            _visitor = visitor;
            _searchEngine = searchEngine;
            _dispatcher = dispatcher;
            _childRequestsProvider = childRequestsProvider;
        }

        private SearchStep CurrentStep => _searchEngine.Steps.Last();

        private List<IRequest> SearchItems => _searchEngine.Items;

        private IRequest ParentRequest
        {
            get
            {
                var parentStep = _searchEngine.Steps.ElementAtOrDefault(_searchEngine.Steps.Count - 2);
                return parentStep?.Request;
            }
        }

        public void Dispatch(TRequest request, ActionType actionType)
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
            var selectedRequest = _searchEngine.SelectedItem;

            LoadChildrenIfNecessary(request);

            if (request != selectedRequest)
            {
                Visit(request, selectedRequest, ActionType.Execute);
                return;
            }

            if (!GetChildren(request).Any())
                _searchEngine.Hide();

            Execute(request);
        }

        private void Execute(TRequest request)
        {
            Visit(request, Request.Null, ActionType.Execute);
        }

        private void Visit(TRequest request, Request childRequest, ActionType actionType)
        {
            var properChildRequest = childRequest ?? Request.Null;
            var context = new RequestVisitContext(request, properChildRequest, actionType);
            _visitor.Visit(context);
        }

        private void OnMoveNext(TRequest request)
        {
            var currentStep = CurrentStep;

            LoadChildrenIfNecessary(request);

            if (!currentStep.Children.Any())
            {
                OnMoveNextWithNoChildren(request);
                return;
            }

            var child = GetVisitableChild(ActionType.MoveNext);

            if (child != null)
                OnChildExecute(child, ActionType.MoveNext);
        }

        private void OnMoveNextWithNoChildren(TRequest request)
        {
            if (ParentRequest is SelectionRequest<TRequest> selectionRequest)
                selectionRequest.ChildSelected?.Invoke(request);

            RemoveLastStep();
        }

        private void LoadChildrenIfNecessary(TRequest request)
        {
            if (CurrentStep.Children.Count == 0)
                CurrentStep.Children.AddRange(GetChildren(request));
        }

        private IRequest GetVisitableChild(ActionType actionType)
        {
            var context = new DispatchPredicateContext(CurrentStep.Text, actionType, _searchEngine.Context);

            IRequest visitableChild = _searchEngine.SelectedItem;

            if (visitableChild == null || _searchEngine.Items.Count > 1 || !visitableChild.CanAccept(context))
                visitableChild = CurrentStep.Children.FirstOrDefault(_ => _.CanAccept(context));

            return visitableChild;
        }

        private void OnChildExecute(IRequest child, ActionType actionType)
        {
            var step = new SearchStep(child);
            _searchEngine.Steps.Add(step);
            _dispatcher.Dispatch(child, actionType);
        }

        private void OnMovePrevious()
        {
            if (CurrentStep.Text.Length == 0 && _searchEngine.Steps.Count > 2)
            {
                RemoveLastStep();
                _dispatcher.Dispatch(CurrentStep.Request, ActionType.MovePrevious);
            }
            else if (CurrentStep.Text.Length > 0)
            {
                CurrentStep.Text = CurrentStep.Text.Remove(CurrentStep.Text.Length - 1);
                _dispatcher.Dispatch(CurrentStep.Request, ActionType.MoveNext);
            }
        }

        private void RemoveLastStep()
        {
            var lastStep = _searchEngine.Steps.Last();
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