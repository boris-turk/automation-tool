using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Decorators
{
    public class ClearSearchItemsRequestHandlerDecorator<TRequest> : IRequestHandler<TRequest>
        where TRequest : Request
    {
        private readonly IRequestHandler<TRequest> _decoratee;

        private readonly ISearchItemsProvider _searchItemsProvider;

        public ClearSearchItemsRequestHandlerDecorator(IRequestHandler<TRequest> decoratee,
            ISearchItemsProvider searchItemsProvider)
        {
            _decoratee = decoratee;
            _searchItemsProvider = searchItemsProvider;
        }

        public void Handle(TRequest request)
        {
            _decoratee.Handle(request);

            if (request.CanMoveNext)
                _searchItemsProvider.Items.Clear();
        }
    }
}