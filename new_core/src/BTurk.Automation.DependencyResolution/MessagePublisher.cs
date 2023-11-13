using BTurk.Automation.Core.Messages;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

public class MessagePublisher : IMessagePublisher
{
    public MessagePublisher(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

    public void Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
        var handler = Container.GetInstance<IMessageHandler<TMessage>>();
        handler.Handle(message);
    }
}