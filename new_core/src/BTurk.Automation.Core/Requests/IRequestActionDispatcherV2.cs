using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public interface IRequestActionDispatcherV2
{
    void Dispatch(ActionType actionType);
}