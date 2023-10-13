using System.ComponentModel;

namespace BTurk.Automation.Core.Views;

public interface IView
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Execute<TAction>(TAction command) where TAction : IViewAction;
}