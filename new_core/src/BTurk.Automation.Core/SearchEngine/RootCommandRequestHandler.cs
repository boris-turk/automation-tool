namespace BTurk.Automation.Core.SearchEngine
{
    public class RootCommandRequestHandler : IRequestHandler<RootCommandRequest>
    {
        private readonly ISearchEngine _searchEngine;

        public RootCommandRequestHandler(ISearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }

        public void Handle(RootCommandRequest request)
        {
            if (!request.Name.StartsWith(_searchEngine.SearchText.Trim()))
                return;

            _searchEngine.AddItem(request.Name);

            if (_searchEngine.SearchText.EndsWith(" "))
                request.Handled = true;
        }
    }
}