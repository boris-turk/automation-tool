using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class ContextMenuCollection : FileStorage<ContextMenuCollection>, ISerializationFinalizer
    {
        public ContextMenuCollection()
        {
            Menus = new List<ContextMenu>();
        }

        [XmlElement("ContextMenu")]
        public List<ContextMenu> Menus { get; set; }

        public override string StorageFileName => "context_menus.xml";

        public ContextMenu GetItemContextMenu(BaseItem item)
        {
            List<BaseItem> contextMenuItems = GetCommonContextMenusItems().ToList();
            contextMenuItems.AddRange(GetAliasedContextMenuItems(item));
            contextMenuItems.AddRange(GetApplicableContextMenuItems(item));

            var menu = new ContextMenu
            {
                Id = Guid.NewGuid().ToString(),
            };

            menu.Items.AddRange(contextMenuItems.Distinct());

            return menu;
        }

        private IEnumerable<BaseItem> GetCommonContextMenusItems()
        {
            return Menus.Where(x => x.ApplicableToAllItems).SelectMany(x => x.Items);
        }

        private IEnumerable<BaseItem> GetApplicableContextMenuItems(BaseItem item)
        {
            if (item is Menu)
            {
                return Enumerable.Empty<BaseItem>();
            }

            ExecutableItemType itemType = item.GetItemType();
            return Menus.Where(x => x.ApplicableToItemType == itemType).SelectMany(x => x.Items);
        }

        private IEnumerable<BaseItem> GetAliasedContextMenuItems(BaseItem item)
        {
            if (item is Menu)
            {
                return Enumerable.Empty<BaseItem>();
            }

            return (
                from alias in item.GetContextMenuAliases()
                let contextMenu = GetMenuByAlias(alias)
                where contextMenu != null
                select contextMenu)
                .SelectMany(x => x.Items);
        }

        private ContextMenu GetMenuByAlias(string alias)
        {
            return Menus.FirstOrDefault(x => x.Alias == alias);
        }

        public void FinalizeSerialization(string file)
        {
            foreach (ContextMenu menu in Menus)
            {
                menu.FinalizeSerialization(file);
                menu.Items.ForEach(i => i.ParentMenu = menu);
                menu.LoadExecutionTimeStamps();
            }

        }
    }
}