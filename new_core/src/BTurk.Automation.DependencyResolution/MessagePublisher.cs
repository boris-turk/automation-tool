using BTurk.Automation.Core.Messages;

namespace BTurk.Automation.DependencyResolution;

public class MessagePublisher : IMessagePublisher
{
    public void Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
        var handler = Container.GetInstance<IMessageHandler<TMessage>>();
        handler.Handle(message);
    }
}