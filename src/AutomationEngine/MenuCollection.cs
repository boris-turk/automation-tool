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
            return _menus.Single(m => m.Alias == alias);
        }

        public Menu GetApplicationMenu(string applicationTitle)
        {
            return (
                from menu in _menus
                where menu.VisibilityCondition != null
                where menu.VisibilityCondition.Type == VisibilityConditionType.WindowTitleRegex
                where Regex.IsMatch(applicationTitle, menu.VisibilityCondition.Value)
                select menu)
                .FirstOrDefault();
        }
    }
}