namespace BTurk.Automation.Core.Requests
{
    public class CommandRequest : Request
    {
        public CommandRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}