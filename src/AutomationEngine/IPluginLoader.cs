using System.Collections.Generic;

namespace AutomationEngine
{
    public interface IPluginLoader
    {
        string Id { get; }
        List<BaseItem> Load();
    }
}