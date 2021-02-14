using System.Collections.Generic;

namespace BTurk.Automation.Core.SearchEngine
{
    public class EnvironmentContext
    {
        public static readonly EnvironmentContext Empty = new EnvironmentContext("", "");

        public EnvironmentContext(string windowTitle, string windowClass)
        {
            WindowTitle = windowTitle;
            WindowClass = windowClass;
            Paths = new List<string>();
        }

        public string WindowTitle { get; }

        public string WindowClass { get; }

        public List<string> Paths { get; }
    }
}