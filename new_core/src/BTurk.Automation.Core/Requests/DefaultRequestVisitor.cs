using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class DefaultRequestVisitor<TRequest, TChild> : IRequestVisitor<TRequest, TChild>
        where TRequest : IRequest where TChild : IRequest
    {
        private readonly ISearchEngine _searchEngine;
        private readonly ICommandProcessor _commandProcessor;

        public DefaultRequestVisitor(ISearchEngine searchEngine, ICommandProcessor commandProcessor)
        {
            _searchEngine = searchEngine;
            _commandProcessor = commandProcessor;
        }

        public void Visit(RequestVisitContext<TRequest, TChild> context)
        {
            var command = context.ChildRequest.Command;

            if (command != null)
            {
                _searchEngine.Hide();
                _commandProcessor.Process(command);
            }
        }
    }
}