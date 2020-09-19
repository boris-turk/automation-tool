namespace BTurk.Automation.Core.SearchEngine
{
    public class SearchItem
    {
        public SearchItem(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override string ToString() => Text;
    }
}