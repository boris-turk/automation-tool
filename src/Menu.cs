using System;
using System.Collections.Generic;
using System.Linq;
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
        [XmlElement("FileSource", typeof(FileDescriptorContentSource))]
        [XmlElement("AhkFunctionSource", typeof(AhkFunctionContentsSource))]
        public object ContentSource { get; set; }

        [XmlIgnore]
        public string MenuFileName
        {
            get { return _fileName; }
        }

        public string ChildrenDirectory { get; set; }

        public bool ChildrenDirectorySpecified
        {
            get { return !string.IsNullOrWhiteSpace(ChildrenDirectory); }
        }

        public bool ContentSourceSpecified
        {
            get { return ContentSource != null; }
        }

        [XmlElement("ExecutableItem", typeof(ExecutableItem))]
        [XmlElement("FileItem", typeof(FileItem))]
        [XmlElement("Menu", typeof(Menu))]
        public List<BaseItem> Items { get; set; }

        public bool ItemsSpecified
        {
            get { return !ContentSourceSpecified; }
        }

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
                if (menu != null && menu.MenuFileName != null)
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
                var menu = item as Menu;

                if (menu != null && menu.MenuFileName != null)
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

        public void LoadItemsIfNecessary()
        {
            if (ContentSource == null || ContentSource is FileDescriptorContentSource)
            {
                return;
            }

            IItemsLoader loader = _itemsLoaderFactory.GetInstance(ContentSource);
            Items = loader.Load();

            ReplaceContextGroups();
            AssignExecutingMethod();
        }

        public void RemoveItem(BaseItem item)
        {
            Items.RemoveAll(x => x.Id == item.Id);
        }

        public Menu Clone()
        {
            return Cloner.Clone(this);
        }

        public virtual void SaveToFile()
        {
            if (_fileName == null)
            {
                throw new InvalidOperationException("File name not specified.");
            }

            var noDuplicates = new List<BaseItem>();

            foreach (BaseItem item in Items)
            {
                if (noDuplicates.All(x => x.Id != item.Id))
                {
                    noDuplicates.Add(item);
                }
            }

            foreach (ExecutableItem item in GetAllItems().OfType<ExecutableItem>())
            {
                if (item.ExecutingMethodNameAssignedAtRuntime)
                {
                    item.ExecutingMethodName = null;
                }
            }

            if (NameSpecified)
            {
                RemovePrependedMenuNameFromItems();
            }

            Menu rootMenuCollection = new Menu
            {
                Items = noDuplicates
            };
            XmlStorage.Save(_fileName, rootMenuCollection);

            XmlStorage.Save(_fileName, this);
        }

        private void RemovePrependedMenuNameFromItems()
        {
            foreach (BaseItem item in GetAllItems())
            {
                int index = item.Name.IndexOf(Name + " ", StringComparison.Ordinal);
                if (index >= 0)
                {
                    item.Name = item.Name.Substring(Name.Length + 1);
                }
            }
        }

        public static T LoadFromFile<T>(string fileName) where T : Menu, new()
        {
            var menu = XmlStorage.Load<T>(fileName);
            if (menu == null)
            {
                menu = new T
                {
                    _fileName = fileName
                };
            }
            return menu;
        }

        private void PrependRootDirectoryToFileItems()
        {
            foreach (FileItem fileItem in Items.OfType<FileItem>())
            {
                fileItem.Directory = ChildrenDirectory;
            }
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
                ExecutableItem additionalItem = item.Clone();
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
            PrepareLoadedItems();
            LoadChildMenus();
        }

        private void PrepareLoadedItems()
        {
            LoadExecutionTimeStamps();
            PrependRootDirectoryToFileItems();
            ReplaceContextGroups();
            PrependMenuNameToItems();
            AssignExecutingMethod();
        }

        private void AssignExecutingMethod()
        {
            foreach (ExecutableItem item in GetAllItems().OfType<ExecutableItem>())
            {
                if (item.ExecutingMethodName == null)
                {
                    item.ExecutingMethodNameAssignedAtRuntime = true;
                    item.ExecutingMethodName = ExecutingMethodName;
                }
            }
        }

        private void LoadChildMenus()
        {
            Items.OfType<Menu>().ToList().ForEach(menu =>
            {
                var contentSource = menu.ContentSource as FileDescriptorContentSource;
                if (contentSource != null)
                {
                    var properMenu = LoadFromFile<Menu>(contentSource.Path);
                    Items.Remove(menu);
                    Items.Add(properMenu);
                }
            });
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

        private void LoadExecutionTimeStamps()
        {
            Items.ForEach(SetExecutionTimeStamp);
        }

        private void SetExecutionTimeStamp(BaseItem item)
        {
            item.LastAccess = ExecutionTimeStamps.Instance.GetTimeStamp(item.Id);

            var menu = item as Menu;
            if (menu != null)
            {
                menu.LoadExecutionTimeStamps();
            }
        }
    }
}
