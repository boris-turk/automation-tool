using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class MenuStorage
    {
        private readonly string _fileName;

        public MenuStorage(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerable<Menu> LoadMenus()
        {
            var menuCollection = XmlStorage.Load<RootMenuCollection>(_fileName);

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
            XmlStorage.Save(_fileName, menuCollection);
        }
    }
}
