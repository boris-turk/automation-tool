namespace AutomationEngine
{
    public class RawFileItemsLoader : IExecutableItemsLoader
    {
        private readonly RawFileContentsSource _contentSource;

        public RawFileItemsLoader(RawFileContentsSource contentSource)
        {
            _contentSource = contentSource;
        }

        public ExecutableItemsCollection Load()
        {
            return AhkInterop.ExecuteFunction(_contentSource);
        }
    }
}