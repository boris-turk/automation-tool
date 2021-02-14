namespace BTurk.Automation.Core.Messages
{
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}