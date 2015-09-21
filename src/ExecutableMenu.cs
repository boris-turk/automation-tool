using System;
using System.Collections.Generic;
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
            List<ExecutableItem> items = File.ReadAllLines(path)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new ExecutableItem(x))
                .Where(x => x.Name != null && x.Parameter != null)
                .ToList();

            ExecutableItems.AddRange(items);
        }
    }
}