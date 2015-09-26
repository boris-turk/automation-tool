using System;
using System.Collections.Generic;
using System.IO;

namespace Ahk
{
    public class MenuStorage
    {
        private readonly string _path;

        public MenuStorage(string path)
        {
            _path = path;
        }

        public IEnumerable<Menu> Load()
        {
            string[] lines = File.ReadAllLines(_path);

            Menu menu = null;
            foreach (string line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    if (menu != null)
                    {
                        yield return menu;
                        menu = null;
                    }
                    continue;
                }
                if (menu == null)
                {
                    menu = new Menu();
                }
                SetProperty(menu, line);
            }

            if (menu != null)
            {
                yield return menu;
            }
        }

        private void SetProperty(Menu menu, string line)
        {
            int index = line.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return;
            }

            string identifier = line.Substring(0, index).Trim();
            string value = line.Substring(index + 1).Trim();

            if (IsEqual(identifier, "text"))
            {
                menu.Name = value;
            }
        }

        private bool IsEqual(string text1, string text2)
        {
            return string.Compare(text1, text2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public void Save(List<Menu> menus)
        {
        }
    }
}