namespace BTurk.Automation.Core.Messages
{
    public interface IMessagePublisher
    {
        void Publish<TMessage>(TMessage message) where TMessage : IMessage;
    }
}