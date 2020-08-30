using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [Serializable]
    public class MainRequestHandler : IRequestHandler<Request>
    {
        private Command _activeCommand;
        private readonly List<Command> _commands;
        private readonly ISearchItemsProvider _searchItemsProvider;
        private readonly IRequestHandler<CompositeRequest> _compositeRequestHandler;

        public MainRequestHandler(List<Command> commands, IRequestHandler<CompositeRequest> compositeRequestHandler,
            ISearchItemsProvider searchItemsProvider)
        {
            _commands = commands;
            _compositeRequestHandler = compositeRequestHandler;
            _searchItemsProvider = searchItemsProvider;
        }

        protected List<SearchItem> SearchItems => _searchItemsProvider.Items;

        public void Handle(Request request)
        {
            SearchItems.Clear();

            if (_activeCommand != null)
            {
                _compositeRequestHandler.Handle(_activeCommand.Request);

                if (!_activeCommand.CanMoveNext)
                    _activeCommand = null;
            }

            SearchItems.Clear();

            foreach (var command in _commands)
            {
                var items = SearchItems.ToList();

                _compositeRequestHandler.Handle(command.Request);

                if (command.Request.Requests.Any(_ => _.CanMoveNext))
                {
                    SearchItems.RemoveAll(_ => items.Contains(_));
                    _activeCommand = command;
                    return;
                }
            }
        }
    }
}
