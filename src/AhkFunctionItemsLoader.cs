using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class AhkFunctionItemsLoader : IExecutableItemsLoader
    {
        private readonly AhkFunctionContentsSource _contentSource;

        public AhkFunctionItemsLoader(AhkFunctionContentsSource contentSource)
        {
            _contentSource = contentSource;
        }

        public List<ExecutableItem> Load()
        {
            return AhkInterop.ExecuteFunction(_contentSource).ToList();
        }
    }
}