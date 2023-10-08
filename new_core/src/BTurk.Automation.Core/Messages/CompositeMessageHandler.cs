using System.Collections.Generic;

namespace BTurk.Automation.Core.Messages;

public class CompositeMessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IMessage
{
    private readonly IEnumerable<IMessageHandler<TMessage>> _handlers;

    public CompositeMessageHandler(IEnumerable<IMessageHandler<TMessage>> handlers)
    {
        _handlers = handlers;
    }

    public void Handle(TMessage message)
    {
        foreach (var handler in _handlers)
            handler.Handle(message);
    }
}