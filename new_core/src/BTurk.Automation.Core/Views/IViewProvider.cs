using System.ComponentModel;

namespace BTurk.Automation.Core.Views;

public interface IViewProvider
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    IView Create(ViewConfiguration configuration);
}