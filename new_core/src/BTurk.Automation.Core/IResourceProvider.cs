namespace BTurk.Automation.Core
{
    public interface IResourceProvider
    {
        T Load<T>(string resourceName);
    }
}