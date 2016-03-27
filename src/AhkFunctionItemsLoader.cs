using System.Collections.Generic;

namespace AutomationEngine
{
    public class AhkFunctionItemsLoader : IItemsLoader
    {
        private readonly AhkFunctionContentsSource _contentSource;

        public AhkFunctionItemsLoader(AhkFunctionContentsSource contentSource)
        {
            _contentSource = contentSource;
        }

        public List<BaseItem> Load()
        {
            return AhkInterop.ExecuteFunction(_contentSource);
        }
    }
}