using System.Text.RegularExpressions;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
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

            if (!request.Name.StartsWith(match.Groups["command"].Value))
                return;

            _searchEngine.AddItem(request.Name);
            request.CanMoveNext = match.Groups["space"].Success;
        }
    }
}