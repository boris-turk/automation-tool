using BTurk.Automation.Core.Messages;

namespace BTurk.Automation.Core.UnitTests;

public class FakeMessagePublisher : IMessagePublisher
{
    public void Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
    }
}