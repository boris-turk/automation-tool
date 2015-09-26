using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class MenuStorage
    {
        private readonly string _path;

        public MenuStorage(string path)
        {
            _path = path;
        }

        private IEnumerable<T> LoadItems<T>(Mappings<T> mappings) where T : class, new()
        {
            string[] lines = File.ReadAllLines(_path);

            T entity = null;
            foreach (string line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    if (entity != null)
                    {
                        yield return entity;
                        entity = null;
                    }
                    continue;
                }
                if (entity == null)
                {
                    entity = new T();
                }
                SetProperty(entity, line, mappings);
            }

            if (entity != null)
            {
                yield return entity;
            }
        }

        public IEnumerable<ExecutableItem> LoadExecutableItems()
        {
            var mappings = new Mappings<ExecutableItem>();
            mappings.Items.Add("name", (x, y) => x.Name = y);
            mappings.Items.Add("value", (x, y) => x.Value = y);
            return LoadItems(mappings);
        }

        public IEnumerable<Menu> LoadMenus()
        {
            var mappings = new Mappings<Menu>();
            return LoadItems<Menu>(mappings);
        }

        private void SetProperty<T>(T entity, string line, Mappings<T> mapping)
        {
            int index = line.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return;
            }

            string identifier = line.Substring(0, index).Trim();
            string value = line.Substring(index + 1).Trim();

            mapping.SetProperty(entity, identifier, value);
        }

        public void Save(List<Menu> menus)
        {
        }

        private class Mappings<T>
        {
            public Dictionary<string, Action<T, string>> Items { get; private set; }

            public Mappings()
            {
                Items = new Dictionary<string, Action<T, string>>();
            }

            public void SetProperty(T entity, string identifier, string value)
            {
                Action<T, string> action = Items.
                    Where(x => IsEqual(x.Key, identifier)).
                    Select(x => x.Value).
                    FirstOrDefault();

                if (action != null)
                {
                    action(entity, value);
                }
            }

            private bool IsEqual(string text1, string text2)
            {
                return string.Compare(text1, text2, StringComparison.OrdinalIgnoreCase) == 0;
            }
        }
    }
}