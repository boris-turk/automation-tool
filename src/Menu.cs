using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class Menu : BaseItem
    {
        private string _name;
        private readonly ItemsLoaderFactory _itemsLoaderFactory;
        private string _fileName;

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

        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }

        [XmlElement("ExecutableItem", typeof(ExecutableItem))]
        [XmlElement("FileItem", typeof(FileItem))]
        [XmlElement("Menu", typeof(Menu))]
        public List<BaseItem> Items { get; set; }

        public bool ItemsSpecified
        {
            get { return !ContentSourceSpecified; }
        }

        public void LoadItems()
        {
            if (ContentSource == null)
            {
                return;
            }

            IItemsLoader loader = _itemsLoaderFactory.GetInstance(ContentSource);
            Items = loader.Load();
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
            menu._fileName = fileName;
            PrependRootDirectoryToFileItems(menu);
            ExpandContextGroups(menu);
            return menu;
        }

        private static void PrependRootDirectoryToFileItems(Menu executableItems)
        {
            if (executableItems.GroupId == null)
            {
                return;
            }
            foreach (FileItem fileItem in executableItems.Items.OfType<FileItem>())
            {
                fileItem.Directory = executableItems.Group.Directory;
            }
        }

        private static void ExpandContextGroups(Menu executableItems)
        {
            List<ExecutableItem> itemsWithContextGroup = executableItems.Items
                .Where(x => x.ContextGroupIdSpecified)
                .OfType<ExecutableItem>()
                .ToList();

            executableItems.Items.RemoveAll(x => itemsWithContextGroup.Contains(x));

            List<BaseItem> replacements = new List<BaseItem>();

            foreach (ExecutableItem item in itemsWithContextGroup)
            {
                List<BaseItem> expandedItems = GetExpandContextGroupItem(item).ToList();
                foreach (BaseItem expandedItem in expandedItems)
                {
                    expandedItem.Name = ReplacePlaceholders(expandedItem);
                }
                replacements.AddRange(expandedItems);
            }

            executableItems.Items.AddRange(replacements);
        }

        private static string ReplacePlaceholders(BaseItem expandedItem)
        {
            return expandedItem.Name.Replace(Configuration.ContextPlaceholder, expandedItem.Context);
        }

        private static IEnumerable<BaseItem> GetExpandContextGroupItem(ExecutableItem item)
        {
            foreach (string context in item.ContextGroup.Contexts)
            {
                BaseItem additionalItem = item.Clone();
                additionalItem.ContextGroupId = null;
                additionalItem.Context = context;
                yield return additionalItem;
            }
        }
    }
}
