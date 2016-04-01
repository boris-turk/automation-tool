using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class Menu : BaseItem, ISerializationFinalizer
    {
        private string _name;
        private readonly ItemsLoaderFactory _itemsLoaderFactory;
        private string _fileName;
        private List<ExecutableItem> _replacedItems;
        private List<BaseItem> _replacementItems;

        public Menu()
        {
            _itemsLoaderFactory = new ItemsLoaderFactory();
        }

        public override string Name
        {
            get
            {
                if (_name == null)
                {
                    return Id;
                }
                return _name;
            }
            set { _name = value; }
        }

        public bool NameSpecified
        {
            get { return _name != null; }
        }

        public string GroupId { get; set; }

        public FileGroup Group
        {
            get { return FileGroupCollection.Instance.GetGroupById(GroupId); }
        }

        public bool GroupSpecified
        {
            get { return !string.IsNullOrWhiteSpace(GroupId); }
        }

        [XmlElement("RawFileSource", typeof(RawFileContentsSource))]
        [XmlElement("FileSource", typeof(FileDescriptorContentSource))]
        [XmlElement("AhkFunctionSource", typeof(AhkFunctionContentsSource))]
        public object ContentSource { get; set; }

        public bool ContentSourceSpecified
        {
            get { return ContentSource != null; }
        }

        public string ExecutingMethodName { get; set; }

        [XmlElement("ExecutableItem", typeof(ExecutableItem))]
        [XmlElement("FileItem", typeof(FileItem))]
        [XmlElement("Menu", typeof(Menu))]
        public List<BaseItem> Items { get; set; }

        public bool ItemsSpecified
        {
            get { return !ContentSourceSpecified; }
        }

        public IEnumerable<BaseItem> GetSelectableItems()
        {
            foreach (BaseItem item in Items)
            {
                if (_replacedItems == null || !_replacedItems.Contains(item))
                {
                    yield return item;
                }
            }
            if (_replacementItems == null)
            {
                yield break;
            }
            foreach (ExecutableItem executableItem in _replacementItems)
            {
                yield return executableItem;
            }
        }

        public void LoadItems()
        {
            if (ContentSource == null)
            {
                return;
            }

            IItemsLoader loader = _itemsLoaderFactory.GetInstance(ContentSource);
            Items = loader.Load();
            PrepareLoadedItems();
        }

        public void RemoveItem(BaseItem item)
        {
            Items.RemoveAll(x => x.Id == item.Id);
        }

        public Menu Clone()
        {
            return Cloner.Clone(this);
        }

        public void SaveToFile()
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

            Menu rootMenuCollection = new Menu
            {
                Items = noDuplicates
            };
            XmlStorage.Save(_fileName, rootMenuCollection);

            XmlStorage.Save(_fileName, this);
        }

        public static Menu LoadFromFile(string fileName)
        {
            var menu = XmlStorage.Load<Menu>(fileName);
            if (menu == null)
            {
                menu = new Menu();
            }
            return menu;
        }

        private void PrependRootDirectoryToFileItems()
        {
            if (GroupId == null)
            {
                return;
            }
            foreach (FileItem fileItem in Items.OfType<FileItem>())
            {
                fileItem.Directory = Group.Directory;
            }
        }

        private void ReplaceContextGroups()
        {
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
                    replacement.Name = ReplacePlaceholders(replacement);
                }
                _replacementItems.AddRange(replacements);
            }
        }

        private string ReplacePlaceholders(BaseItem replacement)
        {
            return replacement.Name.Replace(Configuration.ContextPlaceholder, replacement.Context);
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
        }

        private void PrepareLoadedItems()
        {
            LoadExecutionTimeStamps();
            PrependRootDirectoryToFileItems();
            ReplaceContextGroups();
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
