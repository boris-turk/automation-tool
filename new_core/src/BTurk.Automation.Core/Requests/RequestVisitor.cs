﻿using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class RequestVisitor<TRequest> : IRequestVisitor<TRequest> where TRequest : class, IRequest
    {
        private readonly ISearchEngine _searchEngine;
        private readonly IRequestVisitor _requestVisitor;
        private readonly IRequestExecutor<TRequest> _requestExecutor;
        private readonly IChildRequestsProvider _childRequestsProvider;

        public RequestVisitor(ISearchEngine searchEngine, IRequestVisitor requestVisitor,
            IChildRequestsProvider childRequestsProvider, IRequestExecutor<TRequest> requestExecutor)
        {
            _searchEngine = searchEngine;
            _requestExecutor = requestExecutor;
            _requestVisitor = requestVisitor;
            _childRequestsProvider = childRequestsProvider;
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
            var selectedRequest = _searchEngine.SelectedItem;

            LoadChildrenIfNecessary(request);

            if (request != selectedRequest)
            {
                _requestVisitor.Visit(selectedRequest, ActionType.Execute);
                return;
            }

            if (!GetChildren(request).Any())
                _searchEngine.Hide();

            Execute(request);
        }

        private void Execute(TRequest request)
        {
            _requestExecutor.Execute(request);

            var parentStep = _searchEngine.Steps.ElementAtOrDefault(_searchEngine.Steps.Count - 1);
            var parentRequest = parentStep?.Request;

            if (parentRequest is SelectionRequest<TRequest> selectionRequest)
                selectionRequest.Selected?.Invoke(request);
        }

        private void OnMoveNext(TRequest request)
        {
            var currentStep = CurrentStep;

            LoadChildrenIfNecessary(request);

            if (!currentStep.Children.Any())
            {
                RemoveLastStep();
                return;
            }

            Execute(request);

            var visitableChild = GetVisitableChild();

            if (visitableChild != null)
                VisitChild(visitableChild, ActionType.MoveNext);
        }

        private void LoadChildrenIfNecessary(TRequest request)
        {
            if (CurrentStep.Children.Count == 0)
                CurrentStep.Children.AddRange(GetChildren(request));
        }

        private IRequest GetVisitableChild()
        {
            var context = new VisitPredicateContext(CurrentStep.Text, ActionType.MoveNext, _searchEngine.Context);

            IRequest visitableChild = _searchEngine.SelectedItem;

            if (visitableChild == null || _searchEngine.Items.Count > 1 || !visitableChild.CanVisit(context))
                visitableChild = CurrentStep.Children.FirstOrDefault(_ => _.CanVisit(context));

            return visitableChild;
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