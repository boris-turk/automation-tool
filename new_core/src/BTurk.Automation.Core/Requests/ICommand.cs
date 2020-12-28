namespace BTurk.Automation.Core.Requests
{
    public interface ICommand
    {
        CompositeRequest Request { get; }
    }
}