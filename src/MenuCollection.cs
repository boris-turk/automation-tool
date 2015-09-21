using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class MenuCollection
    {
        public List<Menu> Menus { get; }

        public MenuCollection()
        {
            Menus = new List<Menu>();
            Initialize();
        }

        private void Initialize()
        {
            AddOpenMenu();
            AddPasteMenu();
        }

        private void AddPasteMenu()
        {
        }

        private void AddOpenMenu()
        {
            Menus.Add(new OpenMenu());
            Menus.Add(new PasteMenu());
        }

        public IEnumerable<Menu> GetOrderedMenuItems()
        {
            return Menus.OrderBy(x => x, new MenuItemComparator());
        }
    }
}