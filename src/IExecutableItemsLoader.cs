using System.Collections.Generic;

namespace AutomationEngine
{
    public interface IExecutableItemsLoader
    {
        List<ExecutableItem> Load();
    }
}