using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class Menu
    {
        private string _name;
        private ExecutableItemsCollection _executableItemsCollection;
        private readonly ExecutableItemsLoaderFactory _executableItemsLoaderFactory;

        public Menu()
        {
            _executableItemsCollection = new ExecutableItemsCollection();
            _executableItemsLoaderFactory = new ExecutableItemsLoaderFactory();
            Submenus = new List<Menu>();
            SubmenuIdentifiers = new List<string>();
        }

        public string Id { get; set; }

        public string Name
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

        [XmlElement("RawFileSource", typeof(RawFileContentsSource))]
        [XmlElement("FileSource", typeof(FileDescriptorContentSource))]
        [XmlElement("AhkFunctionSource", typeof(AhkFunctionContentsSource))]
        public object ContentSource { get; set; }

        public string ExecutingMethodName { get; set; }

        [XmlArray("Submenus"), XmlArrayItem("Id")]
        public List<string> SubmenuIdentifiers { get; set; }

        public bool SubmenuIdentifiersSpecified
        {
            get { return SubmenuIdentifiers.Count > 0; }
        }

        [XmlIgnore]
        public List<Menu> Submenus { get; set; }

        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }

        [XmlIgnore]
        public ExecutableItemsCollection ExecutableItemsCollection
        {
            get { return _executableItemsCollection; }
        }

        public string Context { get; set; }

        public bool IsExecutableMenu
        {
            get { return ContentSource != null; }
        }

        public bool ContainsSingleSubmenuWithExecutableItems
        {
            get { return Submenus.Count(x => x.IsExecutableMenu) == 1; }
        }

        public Menu SingleSubmenuWithExecutableItems
        {
            get { return Submenus.Single(x => x.IsExecutableMenu); }
        }

        public void LoadExecutingItems()
        {
            if (ContentSource == null)
            {
                return;
            }

            IExecutableItemsLoader loader = _executableItemsLoaderFactory.GetInstance(ContentSource);
            _executableItemsCollection = loader.Load();
        }

        public void RemoveSubmenu(Menu selectedMenu)
        {
            Submenus.RemoveAll(x => x.Id == selectedMenu.Id);
            SubmenuIdentifiers.RemoveAll(x => x == selectedMenu.Id);
        }

        public Menu Clone()
        {
            return Cloner.Clone(this);
        }
    }
}
