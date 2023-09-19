using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationEngine
{
    public class MenuCollection
    {
        private List<Menu> _menus;

        public static MenuCollection Instance { get; } = new MenuCollection();

        public void LoadMenusFromDisk()
        {
            _menus = new List<Menu>();
            _menus.AddRange(LoadMenus<Menu>(Configuration.Instance.MenuPaths));
            _menus.AddRange(LoadMenus<ApplicationMenu>(Configuration.Instance.ApplicationMenuPaths));
        }

        public void Initialize()
        {
            _menus.ForEach(m => m.LoadChildMenus());
        }

        private IEnumerable<T> LoadMenus<T>(List<string> pathPatterns)
        {
            return
                from pattern in pathPatterns
                from path in ExpandToPaths(pattern)
                let menu = XmlStorage.Load<T>(path)
                where menu != null
                select menu;
        }

        private IEnumerable<string> ExpandToPaths(string pattern)
        {
            if (!pattern.Contains("*"))
            {
                yield return pattern;
                yield break;
            }

            string directory = Path.GetDirectoryName(pattern);
            string filePattern = Path.GetFileName(pattern);

            foreach (string path in Directory.GetFiles(directory, filePattern))
            {
                yield return path;
            }
        }

        public Menu GetRootMenu()
        {
            return GetMenuByAlias(Configuration.Instance.RootMenuAlias);
        }

        public Menu GetMenuByAlias(string alias)
        {
            return GetMenuByAlias(alias, _menus);
        }

        private Menu GetMenuByAlias(string alias, List<Menu> candidates)
        {
            Menu candidate = candidates.SingleOrDefault(m => m.Alias == alias);
            if (candidate != null)
            {
                return candidate;
            }
            foreach (Menu menu in candidates)
            {
                candidate = GetMenuByAlias(alias, menu.Items.OfType<Menu>().ToList());
                if (candidate != null)
                {
                    return candidate;
                }
            }
            return null;
        }

        public BaseItem GetItemById(string id)
        {
            return (
                from menu in _menus
                from item in menu.ChildItems()
                where item.Id == id && item.ClonedFrom == null
                select item)
                .Single();
        }

        public Menu GetApplicationMenu(string applicationTitle)
        {
            var applicationMenu = (
                    from menu in _menus
                    where menu.VisibilityConditions != null
                    where menu.VisibilityConditions.Any()
                    where menu.IsVisible(applicationTitle)
                    select menu)
                .FirstOrDefault();

            return applicationMenu;
        }
    }
}