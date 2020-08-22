using System;

namespace BTurk.Automation.Host.SearchEngine
{
    [Serializable]
    public class SearchParameters
    {
        public SearchParameters(string text, ActionType actionType)
        {
            Text = text;
            ActionType = actionType;
        }

        public ActionType ActionType { get; }

        public string Text { get; }
    }
}
