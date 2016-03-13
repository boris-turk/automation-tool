using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class MenuStorage
    {
        private readonly string _fileName;

        public MenuStorage()
        {
            _fileName = Configuration.MenusFileName;
        }

        public Menu LoadMenuStructure()
        {
            var menuCollection = XmlStorage.Load<RootMenuCollection>(_fileName);

            var rootMenu = new Menu
            {
                Id = "root"
            };
            rootMenu.Submenus.AddRange(menuCollection.Menus);

            foreach (Menu menu in menuCollection.Menus)
            {
                List<Menu> subMenus = menu.SubmenuIdentifiers
                    .Select(x => menuCollection.Menus.FirstOrDefault(y => y.Id == x))
                    .Where(x => x != null)
                    .ToList();

                menu.Submenus.AddRange(subMenus);
            }

            return rootMenu;
        }

        public void SaveMenuStructure(Menu rootMenu)
        {
            var menusWithoutDuplicates = new List<Menu>();

            foreach (Menu menu in rootMenu.Submenus)
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
