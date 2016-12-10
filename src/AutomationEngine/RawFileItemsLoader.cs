using System.Collections.Generic;

namespace AutomationEngine
{
    public class RawFileItemsLoader : IItemsLoader
    {
        private readonly RawFileContentsSource _contentSource;

        public RawFileItemsLoader(RawFileContentsSource contentSource)
        {
            _contentSource = contentSource;
        }

        public List<BaseItem> Load()
        {
            return AhkInterop.ExecuteFunction(_contentSource);
        }
    }
}