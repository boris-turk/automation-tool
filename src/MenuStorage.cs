using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class MenuStorage
    {
        private readonly string _rootMenuPath;

        public MenuStorage(string fileName)
        {
            _rootMenuPath = Path.Combine(Configuration.RootDirectory, fileName);
        }

        public IEnumerable<ExecutableItem> LoadExecutableItems()
        {
            var executableItems = XmlStorage.Load<ExecutableItemsCollection>(_rootMenuPath);
            return executableItems.Items;
        }

        public IEnumerable<Menu> LoadMenus()
        {
            var menuCollection = XmlStorage.Load<RootMenuCollection>(_rootMenuPath);
            return menuCollection.Menus;
        }

        public void SaveMenus(List<Menu> menus)
        {
            RootMenuCollection menuCollection = new RootMenuCollection
            {
                Menus = menus
            };
            XmlStorage.Save(_rootMenuPath, menuCollection);
        }

        public void SaveExecutableItems(List<ExecutableItem> items)
        {
            ExecutableItemsCollection menuCollection = new ExecutableItemsCollection
            {
                Items = items
            };
            XmlStorage.Save(_rootMenuPath, menuCollection);
        }
    }
}
