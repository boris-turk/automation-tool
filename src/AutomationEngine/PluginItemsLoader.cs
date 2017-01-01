using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class PluginItemsLoader : IItemsLoader
    {
        private readonly string _sourceId;

        public PluginItemsLoader(string sourceId)
        {
            _sourceId = sourceId;
        }

        public List<BaseItem> Load()
        {
            IPluginLoader loader = PluginsCollection.Instance.PluginLoaders.SingleOrDefault(x => x.Id == _sourceId);
            if (loader == null)
            {
                return new List<BaseItem>();
            }

            List<BaseItem> items = loader.Load();

            DateTime now = DateTime.Now;
            foreach (BaseItem item in items)
            {
                item.Id = null;
                item.ParentMenu = MenuEngine.Instance.SelectedItem as Menu;
                if (item.LastAccess == DateTime.MinValue)
                {
                    now = now.AddTicks(-1);
                    item.LastAccess = now;
                }
            }

            return items;
        }
    }
}