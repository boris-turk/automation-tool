namespace BTurk.Automation.Core.SearchEngine
{
    public class WindowContext
    {
        public static readonly WindowContext Empty = new WindowContext("", "");

        public WindowContext(string title, string className)
        {
            Title = title;
            Class = className;
        }

        public string Title { get; }

        public string Class { get; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(Title);
    }
}