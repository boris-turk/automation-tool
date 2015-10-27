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

        public Menu()
        {
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

        public string ContentsFileName { get; set; }

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
            get
            {
                if (_executableItems == null)
                {
                    LoadExecutingItems();
                }
                return _executableItems;
            }
        }

        private void LoadExecutingItems()
        {
            if (string.IsNullOrWhiteSpace(ContentsFileName))
            {
                _executableItems = new List<ExecutableItem>();
            }
            else
            {
                _executableItems = new MenuStorage(ContentsFileName).LoadExecutableItems().ToList();
            }
        }

        public Menu Clone()
        {
            return Cloner.Clone(this);
        }
    }
}
