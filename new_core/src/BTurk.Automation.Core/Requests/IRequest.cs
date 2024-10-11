using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.Requests;

public interface IRequest : IRequestV2
{
    ICommand Command { get; }
    string Text { get; }
    bool CanAccept(DispatchPredicateContext context);
}