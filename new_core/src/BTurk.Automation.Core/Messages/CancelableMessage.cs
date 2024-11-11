namespace BTurk.Automation.Core.Messages
{
    public abstract class CancelableMessage : IMessage
    {
        public bool Canceled { get; set; }
    }
}