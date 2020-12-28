using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Decorators
{
    public class FilteredRequestHandlerDecorator<TRequest> : IRequestHandler<TRequest>
        where TRequest : IFilteredRequest
    {
        private readonly ISearchEngine _searchEngine;
        private readonly IRequestHandler<TRequest> _decoratee;

        public FilteredRequestHandlerDecorator(IRequestHandler<TRequest> decoratee, ISearchEngine searchEngine)
        {
            _decoratee = decoratee;
            _searchEngine = searchEngine;
        }

        public void Handle(TRequest request)
        {
            _decoratee.Handle(request);
            _searchEngine.FilterText = GetFilterText(request);
        }

        private string GetFilterText(TRequest request)
        {
            var filterText = _searchEngine.SearchText;

            if (request.FilterTextProvider != null)
                filterText = request.FilterTextProvider.Invoke(filterText);

            return filterText;
        }
    }
}