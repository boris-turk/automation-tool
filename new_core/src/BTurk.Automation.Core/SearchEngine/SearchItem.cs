using System;

namespace BTurk.Automation.Core.SearchEngine
{
    [Serializable]
    public class SearchItem
    {
        public string Text { get; set; }
        public bool IsFilteredOut { get; set; }
    }
}
