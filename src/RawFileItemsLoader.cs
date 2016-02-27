using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class RawFileItemsLoader : IExecutableItemsLoader
    {
        private readonly RawFileContentsSource _contentSource;

        public RawFileItemsLoader(RawFileContentsSource contentSource)
        {
            _contentSource = contentSource;
        }

        public List<ExecutableItem> Load()
        {
            return AhkInterop.ExecuteFunction(_contentSource).ToList();
        }
    }
}