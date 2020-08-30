namespace BTurk.Automation.Core.SearchEngine
{
    public class ClearSearchItemsRequestHandlerDecorator<TRequest> : IRequestHandler<TRequest>
        where TRequest : SequentialRequest
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
            if (request.CanMoveNext)
                _searchItemsProvider.Items.Clear();

            _decoratee.Handle(request);
        }
    }
}