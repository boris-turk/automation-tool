using System;

namespace BTurk.Automation.Core.SearchEngine
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
