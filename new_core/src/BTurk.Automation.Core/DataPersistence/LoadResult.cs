namespace BTurk.Automation.Core.DataPersistence;

public class LoadResult<TInstance>
{
    public LoadResult(TInstance instance)
    {
        Instance = instance;
    }

    public TInstance Instance { get; }
}