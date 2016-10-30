using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class ContextMenuCollection : FileStorage<ContextMenuCollection>, ISerializationFinalizer
    {
        public ContextMenuCollection()
        {
            Menus = new List<Menu>();
        }

        [XmlElement("Menu")]
        public List<Menu> Menus { get; set; }

        public override string StorageFileName => "context_menus.xml";

        public Menu GetItemContextMenu(BaseItem item)
        {
            List<BaseItem> contextMenuItems = (
                from alias in item.GetContextMenuAliases()
                let contextMenu = GetMenuByAlias(alias)
                where contextMenu != null
                select contextMenu)
                .SelectMany(x => x.Items)
                .Distinct()
                .ToList();

            var menu = new Menu
            {
                Id = Guid.NewGuid().ToString(),
            };

            menu.Items.AddRange(contextMenuItems);

            return menu;
        }

        private Menu GetMenuByAlias(string alias)
        {
            return Menus.FirstOrDefault(x => x.Alias == alias);
        }

        public void FinalizeSerialization(string file)
        {
            foreach (Menu menu in Menus)
            {
                menu.LoadExecutionTimeStamps();
            }

        }
    }
}