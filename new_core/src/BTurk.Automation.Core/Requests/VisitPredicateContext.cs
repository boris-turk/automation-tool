using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class VisitPredicateContext
    {
        public VisitPredicateContext(string text, ActionType actionType, EnvironmentContext environmentContext)
        {
            Text = text;
            ActionType = actionType;
            EnvironmentContext = environmentContext;
        }

        public string Text { get; }

        public ActionType ActionType { get; }

        public EnvironmentContext EnvironmentContext { get; }
    }
}