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
        private List<ExecutableItem> _executableItems;
        private readonly ExecutableItemsLoaderFactory _executableItemsLoaderFactory;

        public Menu()
        {
            _executableItems = new List<ExecutableItem>();
            _executableItemsLoaderFactory = new ExecutableItemsLoaderFactory();
            Aliases = new List<string>();
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

        [XmlArray("Aliases"), XmlArrayItem("Alias")]
        public List<string> Aliases { get; set; }

        public bool AliasesSpecified
        {
            get { return Aliases.Count > 0; }
        }

        public bool NameSpecified
        {
            get { return _name != null; }
        }

        [XmlElement("RawFileSource", typeof(RawFileContentsSource))]
        [XmlElement("FileSource", typeof(FileDescriptorContentSource))]
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
        public List<ExecutableItem> ExecutableItems
        {
            get { return _executableItems; }
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
            _executableItems = loader.Load();
        }

        public Menu Clone()
        {
            return Cloner.Clone(this);
        }
    }
}
