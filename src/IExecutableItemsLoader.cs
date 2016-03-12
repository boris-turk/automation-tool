using System.Collections.Generic;

namespace AutomationEngine
{
    public interface IExecutableItemsLoader
    {
        ExecutableItemsCollection Load();
    }
}