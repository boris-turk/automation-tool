namespace AutomationEngine
{
    public class AhkFunctionItemsLoader : IExecutableItemsLoader
    {
        private readonly AhkFunctionContentsSource _contentSource;

        public AhkFunctionItemsLoader(AhkFunctionContentsSource contentSource)
        {
            _contentSource = contentSource;
        }

        public ExecutableItemsCollection Load()
        {
            return AhkInterop.ExecuteFunction(_contentSource);
        }
    }
}