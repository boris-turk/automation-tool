namespace BTurk.Automation.Core.SearchEngine
{
    public class ClearSearchItemsRequestHandlerDecorator<TRequest> : IRequestHandler<TRequest>
        where TRequest : Request, IClearSearchItemsRequest
    {
        private readonly ISearchEngine _searchEngine;

        private readonly IRequestHandler<TRequest> _decoratee;

        private readonly ISearchItemsProvider _searchItemsProvider;

        public ClearSearchItemsRequestHandlerDecorator(IRequestHandler<TRequest> decoratee,
            ISearchEngine searchEngine, ISearchItemsProvider searchItemsProvider)
        {
            _decoratee = decoratee;
            _searchEngine = searchEngine;
            _searchItemsProvider = searchItemsProvider;
        }

        public void Handle(TRequest request)
        {
            if (request.ShouldClearSearchItems(_searchEngine.SearchText))
                _searchItemsProvider.Items.Clear();

            _decoratee.Handle(request);
        }
    }
}