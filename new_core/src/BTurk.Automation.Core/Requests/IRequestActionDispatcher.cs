using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public interface IRequestActionDispatcher
{
    void Dispatch(ActionType actionType);
}