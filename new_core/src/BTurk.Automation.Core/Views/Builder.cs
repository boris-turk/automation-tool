using System.ComponentModel;

namespace BTurk.Automation.Core.Views;

public class Builder<T>
{
    public Builder(T instance)
    {
        Instance = instance;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public T Instance { get; }
}