using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class ContextMenuCollection : FileStorage<ContextMenuCollection>, ISerializationFinalizer
    {
        public ContextMenuCollection()
        {
            Menus = new List<Menu>();
        }

        [XmlElement("ContextMenu")]
        public List<Menu> Menus { get; set; }

        public override string StorageFileName => "context_menus.xml";

        public Menu GetItemContextMenu()
        {
            var menu = new Menu
            {
                Id = Guid.NewGuid().ToString()
            };

            IEnumerable<BaseItem> contextMenuItems = GetContextMenuItems();
            menu.Items.AddRange(contextMenuItems);

            return menu;
        }

        private IEnumerable<BaseItem> GetContextMenuItems()
        {
            return
                from contextMenu in Menus
                where contextMenu != null
                from contextMenuItem in contextMenu.Items.OfType<ExecutableItem>()
                where contextMenuItem.Arguments.All(a => !a.IsEmpty)
                select contextMenuItem;
        }

        public void FinalizeSerialization(string file)
        {
            foreach (Menu menu in Menus)
            {
                menu.FinalizeSerialization(file);
                menu.Items.ForEach(i => i.ParentMenu = menu);
                menu.LoadExecutionTimeStamps();
            }

        }
    }
}