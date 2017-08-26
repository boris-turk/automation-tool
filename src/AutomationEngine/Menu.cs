using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class Menu : BaseItem, ISerializationFinalizer
    {
        private readonly ItemsLoaderFactory _itemsLoaderFactory;
        private string _fileName;
        private List<ExecutableItem> _replacedItems;
        private List<BaseItem> _replacementItems;

        public Menu()
        {
            Items = new List<BaseItem>();
            _itemsLoaderFactory = new ItemsLoaderFactory();
        }

        [XmlElement("RawFileSource", typeof(RawFileContentsSource))]
        [XmlElement("AhkFunctionSource", typeof(AhkFunctionContentsSource))]
        [XmlElement("PluginSource", typeof(PluginContentSource))]
        public object ContentSource { get; set; }

        [XmlIgnore]
        public string MenuFileName => _fileName;

        public bool ContentSourceSpecified => ContentSource != null;

        [XmlElement("ExecutableItem", typeof(ExecutableItem))]
        [XmlElement("FileItem", typeof(FileItem))]
        [XmlElement("Menu", typeof(Menu))]
        [XmlElement("ReferencedMenu", typeof(ReferencedMenu))]
        public List<BaseItem> Items { get; set; }

        public bool ItemsSpecified => !ContentSourceSpecified;

        [XmlArrayItem("Type")]
        public List<ValueType> ArgumentTypes { get; set; }

        public IEnumerable<BaseItem> GetAllItems()
        {
            IEnumerable<BaseItem> allItems = Items;
            if (_replacementItems != null)
            {
                allItems = Items.Union(_replacementItems).Union(_replacedItems);
            }

            foreach (BaseItem item in allItems)
            {
                var menu = item as Menu;
                if (menu?.MenuFileName != null)
                {
                    foreach (BaseItem childItem in menu.GetAllItems())
                    {
                        yield return childItem;
                    }
                }
                yield return item;
            }
        }

        public virtual IEnumerable<BaseItem> GetSelectableItems()
        {
            foreach (BaseItem item in Items)
            {
                if (!IsItemVisible(item) || IsMergedIntoParent(item))
                {
                    continue;
                }

                var menu = item as Menu;

                if (menu?.MenuFileName != null)
                {
                    foreach (BaseItem childMenuItem in menu.GetSelectableItems())
                    {
                        yield return childMenuItem;
                    }
                    continue;
                }

                if (_replacedItems == null || !_replacedItems.Contains(item))
                {
                    yield return item;
                }
            }
            if (_replacementItems == null)
            {
                yield break;
            }
            foreach (BaseItem executableItem in _replacementItems)
            {
                yield return executableItem;
            }
        }

        private bool IsMergedIntoParent(BaseItem item)
        {
            return (item as ReferencedMenu)?.MergedIntoParent ?? false;
        }

        private bool IsItemVisible(BaseItem item)
        {
            if (item.VisibilityCondition?.Type == VisibilityConditionType.WindowTitleRegex)
            {
                string title = MenuEngine.Instance.ApplicationContext;
                return Regex.IsMatch(title, item.VisibilityCondition.Value);
            }
            return true;
        }

        public void LoadItemsIfNecessary()
        {
            if (ContentSource == null)
            {
                return;
            }

            IItemsLoader loader = _itemsLoaderFactory.GetInstance(ContentSource);
            Items = loader.Load();
            Items.ForEach(i => i.ParentMenu = this);

            ReplaceContextGroups();
        }

        public void RemoveItem(BaseItem item)
        {
            Items.RemoveAll(x => x.Id == item.Id);
        }

        public override BaseItem Clone()
        {
            return Cloner.Clone(this);
        }

        private void ReplaceContextGroups()
        {
            foreach (BaseItem item in Items.Where(x => !x.ContextGroupIdSpecified))
            {
                ReplaceContextPlaceholders(item);
            }

            _replacedItems = Items
                .Where(x => x.ContextGroupIdSpecified)
                .OfType<ExecutableItem>()
                .ToList();

            _replacementItems = new List<BaseItem>();

            foreach (ExecutableItem item in _replacedItems)
            {
                List<BaseItem> replacements = CreateReplacementItems(item).ToList();
                foreach (BaseItem replacement in replacements)
                {
                    ReplaceContextPlaceholders(replacement);
                }
                _replacementItems.AddRange(replacements);
            }
        }

        private void ReplaceContextPlaceholders(BaseItem item)
        {
            if (item.Name != null && item.Context != null)
            {
                item.Name = item.Name.Replace(Configuration.ContextPlaceholder, item.Context);
            }
        }

        private IEnumerable<BaseItem> CreateReplacementItems(ExecutableItem item)
        {
            foreach (string context in item.ContextGroup.Contexts)
            {
                ExecutableItem additionalItem = (ExecutableItem)item.Clone();
                additionalItem.Id = Guid.NewGuid().ToString();
                additionalItem.ReplacedItemId = item.Id;
                additionalItem.Context = context;
                additionalItem.LastAccess = item.LastAccess;
                yield return additionalItem;
            }
        }

        public void FinalizeSerialization(string file)
        {
            _fileName = file;
            this.LoadExecutionTimeStamps();
            ReplaceContextGroups();
            PrependMenuNameToItems();
            AssignParentMenu();
        }

        private void AssignParentMenu()
        {
            foreach (BaseItem item in GetAllItems())
            {
                item.PersistenceParentMenu = this;
                item.ParentMenu = this;
            }
        }

        internal void LoadChildMenus()
        {
            Items.OfType<ReferencedMenu>().ToList().ForEach(LoadReferencedMenu);
        }

        private void LoadReferencedMenu(ReferencedMenu menu)
        {
            if (menu.MergedIntoParent)
            {
                Items.AddRange(CloneReferencedMenuItems(menu));
            }
        }

        private IEnumerable<BaseItem> CloneReferencedMenuItems(ReferencedMenu menu)
        {
            Menu properMenu = MenuCollection.Instance.GetMenuByAlias(menu.Alias);
            foreach (BaseItem originalItem in properMenu.GetSelectableItems())
            {
                BaseItem clonedItem = originalItem.Clone();
                clonedItem.Name = menu.GetProperItemName(clonedItem, originalItem);
                clonedItem.ClonedFrom = originalItem;
                clonedItem.PersistenceParentMenu = originalItem.PersistenceParentMenu;
                clonedItem.ParentMenu = this;
                clonedItem.ExecutingMethodName = menu.GetProperExecutingMethodName(originalItem);
                yield return clonedItem;
            }
        }

        private void PrependMenuNameToItems()
        {
            if (!NameSpecified)
            {
                return;
            }
            foreach (BaseItem item in GetAllItems())
            {
                item.Name = Name + " " + item.Name;   
            }
        }
    }
}
