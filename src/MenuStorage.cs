using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class MenuStorage
    {
        private readonly string _path;

        public MenuStorage(string fileName)
        {
            _path = Path.Combine(Configuration.RootDirectory, fileName);
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
            mappings.Items.Add("value", (x, y) => SetArgumentValue(x, y));
            mappings.Items.Add("ahk_value", (x, y) => SetAhkArgumentValue(x, y));
            return LoadItems(mappings);
        }

        private void SetArgumentValue(ExecutableItem item, string value)
        {
            var argument = new ExecutableItemArgument
            {
                Value = value,
                Type = ArgumentType.String
            };
            item.Arguments.Add(argument);
        }

        private void SetAhkArgumentValue(ExecutableItem item, string value)
        {
            var argument = new ExecutableItemArgument
            {
                Value = value,
                Type = ArgumentType.AutoHotkey
            };
            item.Arguments.Add(argument);
        }

        public IEnumerable<Menu> LoadMenus()
        {
            var mappings = new Mappings<Menu>();
            mappings.Items.Add("id", (x, y) => x.Id = y);
            mappings.Items.Add("name", (x, y) => x.Name = y);
            mappings.Items.Add("file", (x, y) => x.ContentsFileName = y);
            mappings.Items.Add("execute", (x, y) => x.ExecutingMethodName = y);
            mappings.Items.Add("access", (x, y) => x.LastAccess = GetLastAccessTimeStamp(y));
            mappings.Items.Add("submenus", (x, y) => x.SubmenusIdentifiers.AddRange(GetSubmenus(y)));
            return LoadItems(mappings);
        }

        private IEnumerable<string> GetSubmenus(string text)
        {
            return text.Split(',').Select(menuId => menuId.Trim());
        }

        private DateTime GetLastAccessTimeStamp(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return DateTime.MinValue;
            }
            DateTime dateTime;
            DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
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
            public Dictionary<string, Action<T, string>> Items { get; }

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

                action?.Invoke(entity, value);
            }

            private bool IsEqual(string text1, string text2)
            {
                return string.Compare(text1, text2, StringComparison.OrdinalIgnoreCase) == 0;
            }
        }
    }
}