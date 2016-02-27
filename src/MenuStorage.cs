using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AutomationEngine
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
            if (executableItems == null)
            {
                return new ExecutableItem[] { };
            }
            PrependRootDirectoryToFileItems(executableItems);
            return executableItems.Items;
        }

        private void PrependRootDirectoryToFileItems(ExecutableItemsCollection executableItems)
        {
            if (executableItems.Group == null)
            {
                return;
            }
            foreach (FileItem fileItem in executableItems.Items.OfType<FileItem>())
            {
                if (fileItem.Arguments.Count == 0)
                {
                    continue;
                }
                string path = Path.Combine(executableItems.Group, fileItem.FilePath);
                fileItem.Arguments[0] = new StringValue { Value = path };
            }
        }

        public IEnumerable<Menu> LoadMenus()
        {
            var menuCollection = XmlStorage.Load<RootMenuCollection>(_rootMenuPath);

            foreach (Menu menu in menuCollection.Menus.ToList())
            {
                foreach (string alias in menu.Aliases)
                {
                    Menu aliasMenu = menu.Clone();
                    aliasMenu.Name = alias;
                    menuCollection.Menus.Add(aliasMenu);
                }
            }

            return menuCollection.Menus;
        }

        public void SaveMenus(List<Menu> menus)
        {
            var menusWithoutDuplicates = new List<Menu>();

            foreach (Menu menu in menus)
            {
                if (menusWithoutDuplicates.All(x => x.Id != menu.Id))
                {
                    menusWithoutDuplicates.Add(menu);
                }
            }

            RootMenuCollection menuCollection = new RootMenuCollection
            {
                Menus = menusWithoutDuplicates
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
