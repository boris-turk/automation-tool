namespace BTurk.Automation.Core.SearchEngine
{
    public class WindowContext
    {
        public WindowContext(string title)
        {
            Title = title;
        }

        public string Title { get; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(Title);
    }
}