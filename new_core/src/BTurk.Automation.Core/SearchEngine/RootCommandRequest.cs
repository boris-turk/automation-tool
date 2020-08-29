namespace BTurk.Automation.Core.SearchEngine
{
    public class RootCommandRequest : Request
    {
        public RootCommandRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}