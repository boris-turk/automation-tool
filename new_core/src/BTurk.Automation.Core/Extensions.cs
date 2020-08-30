using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core
{
    public static class Extensions
    {
        public static void AddItem(this ISearchEngine engine, string text)
        {
            engine.AddItems(new[] {new SearchItem(text)});
        }
    }
}