using System.ComponentModel;

namespace BTurk.Automation.Core.AsyncServices;

public interface IAsyncExecution
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetProgressData(ProgressData data);

    bool IsCanceled { get; }
}