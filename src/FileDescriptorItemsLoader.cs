namespace AutomationEngine
{
    public class FileDescriptorItemsLoader : IExecutableItemsLoader
    {
        private readonly string _path;

        public FileDescriptorItemsLoader(string path)
        {
            _path = path;
        }

        public ExecutableItemsCollection Load()
        {
            return ExecutableItemsCollection.LoadFromFile(_path);
        }
    }
}