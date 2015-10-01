using System;
using System.Collections.Generic;
using System.Linq;

namespace Ahk
{
    public class Menu
    {
        private string _name;

        private List<ExecutableItem> _executableItems;

        public Menu()
        {
            Submenus = new List<Menu>();
            SubmenusIdentifiers = new List<string>();
        }

        public string Id { get; set; }

        public string ContentsFileName { get; set; }

        public string ExecutingMethodName { get; set; }

        public DateTime LastAccess { get; set; }

        public List<string> SubmenusIdentifiers { get; private set; }

        public List<Menu> Submenus { get; private set; }

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
    }
}