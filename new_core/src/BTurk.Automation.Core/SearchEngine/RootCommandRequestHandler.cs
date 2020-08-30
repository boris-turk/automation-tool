using System.Text.RegularExpressions;

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
            var match = Regex.Match(_searchEngine.SearchText, @"^(?<command>\S+)(?<space>\s)?");

            if (!match.Success)
            {
                _searchEngine.AddItem(request.Name);
                return;
            }

            request.Handled = request.Name.StartsWith(match.Groups["command"].Value);
            request.CanMoveNext = match.Groups["space"].Success;

            if (request.Handled)
                _searchEngine.AddItem(request.Name);
        }
    }
}