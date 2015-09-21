using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class Menu
    {
        public string Name { get; set; }
        public List<Menu> SubItems { get; private set; }

        public Menu()
        {
            InitializeSubMenu();
        }

        private void InitializeSubMenu()
        {
            SubItems = new List<Menu>();
            SubItems.AddRange(GetSubmenuItems());
        }

        protected virtual IEnumerable<Menu> GetSubmenuItems()
        {
            return new Menu[] { };
        }
    }
}