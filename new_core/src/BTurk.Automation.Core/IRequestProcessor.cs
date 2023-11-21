using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Queries;

namespace BTurk.Automation.Core;

public interface IRequestProcessor : IQueryProcessor, ICommandProcessor, IMessagePublisher
{
}