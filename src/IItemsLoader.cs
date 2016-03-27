using System.Collections.Generic;

namespace AutomationEngine
{
    public interface IItemsLoader
    {
        List<BaseItem> Load();
    }
}