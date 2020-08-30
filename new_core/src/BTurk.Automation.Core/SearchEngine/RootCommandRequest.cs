namespace BTurk.Automation.Core.SearchEngine
{
    public class RootCommandRequest : SequentialRequest, IClearSearchItemsRequest
    {
        public RootCommandRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool ShouldClearSearchItems(string text) => text.EndsWith(" ");
    }
}