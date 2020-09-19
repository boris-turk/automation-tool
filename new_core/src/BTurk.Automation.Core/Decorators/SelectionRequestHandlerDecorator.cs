using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Decorators
{
    public class SelectionRequestHandlerDecorator<TRequest, TItem> : IRequestHandler<TRequest>
        where TRequest : SelectionRequest<TItem>
        where TItem : SearchItem
    {
        private readonly ISearchEngine _searchEngine;
        private readonly IRequestHandler<TRequest> _decoratee;

        public SelectionRequestHandlerDecorator(IRequestHandler<TRequest> decoratee, ISearchEngine searchEngine)
        {
            _decoratee = decoratee;
            _searchEngine = searchEngine;
        }

        public void Handle(TRequest request)
        {
            _decoratee.Handle(request);

            if (_searchEngine.ActionType != ActionType.Execution)
                return;

            var selectedItem = (TItem) _searchEngine.SelectedItem;

            if (selectedItem != null)
                request.OnSelectedAction?.Invoke(selectedItem);
        }
    }
}