using BTurk.Automation.Core.FileSystem;

namespace BTurk.Automation.Core.DataPersistence;

public interface IResourceProvider
{
    T Load<T>(FileParameters fileParameters);
}