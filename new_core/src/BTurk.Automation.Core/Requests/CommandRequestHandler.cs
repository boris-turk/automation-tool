using System.Text.RegularExpressions;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class CommandRequestHandler : IRequestHandler<CommandRequest>
    {
        private readonly ISearchEngine _searchEngine;

        public CommandRequestHandler(ISearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }

        public void Handle(CommandRequest request)
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