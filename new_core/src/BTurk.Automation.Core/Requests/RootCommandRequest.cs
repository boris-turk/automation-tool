namespace BTurk.Automation.Core.Requests
{
    public class RootCommandRequest : SequentialRequest
    {
        public RootCommandRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}