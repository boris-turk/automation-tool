using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class ExecutableMenu : Menu
    {
        public List<ExecutableItem> ExecutableItems { get; }

        public Action<ExecutableItem> Execute { get; set; }

        public ExecutableMenu()
        {
            ExecutableItems = new List<ExecutableItem>();
        }

        public void LoadItemsFromFile(string path)
        {
            var menuStorage = new MenuStorage(path);
            IEnumerable<ExecutableItem> items = menuStorage.LoadExecutableItems();
            ExecutableItems.AddRange(items);
        }
    }
}