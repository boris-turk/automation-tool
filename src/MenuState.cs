using System;
using System.Collections.Generic;
using System.Linq;

namespace Ahk
{
    public class MenuState
    {
        private readonly Stack<Menu> _menusStack;

        public MenuState(Menu rootMenu)
        {
            _menusStack = new Stack<Menu>();
            _menusStack.Push(rootMenu);
        }

        public List<Menu> MatchingSubmenus
        {
            get
            {
                List<Menu> result = Submenus.Where(x => MatchesFilter(x.Name)).ToList();
                result.Sort(new MenuComparer(this));
                return result;
            }
        }

        public string Filter { get; set; }

        public bool IsSelectionMenu
        {
            get
            {
                if (Submenus.Count > 0)
                {
                    return false;
                }
                return ExecutableItems.Count > 0;
            }
        }

        private List<ExecutableItem> ExecutableItems
        {
            get { return ExecutableMenu.ExecutableItems; }
        }

        private Menu ExecutableMenu
        {
            get
            {
                Menu menu = _menusStack.Peek();

                Menu candidate = _menusStack.Skip(1).FirstOrDefault(
                    x => menu.Submenus.Any(y => y.Id == x.Id));

                if (candidate != null)
                {
                    return candidate;
                }

                if (menu.Submenus.Count(x => x.ExecutableItems.Count > 0) == 1)
                {
                    return menu.Submenus.First();
                }

                return menu;
            }
        }

        public List<ExecutableItem> MatchingExecutableItems
        {
            get
            {
                List<ExecutableItem> result = ExecutableItems.Where(x => MatchesFilter(x.Name)).ToList();
                result.Sort(new ExecutableItemComparer());
                return result;
            }
        }

        public bool IsExecutableItemSelected
        {
            get { return MatchingExecutableItems.Count > SelectedIndex; }
        }

        public int SelectedIndex { get; set; }

        public Menu Menu
        {
            get { return _menusStack.Peek(); }
        }

        public bool IsSubmenuSelected
        {
            get { return MatchingSubmenus.Count > SelectedIndex; }
        }

        private List<Menu> Submenus
        {
            get
            {
                Menu menu = _menusStack.Peek();
                if (_menusStack.Count > 1 && _menusStack.Peek().Submenus.Count(x => x.ExecutableItems.Count > 0) == 1)
                {
                    return new List<Menu>();
                }
                if (_menusStack.Skip(1).Any(x => menu.Submenus.Any(y => y.Id == x.Id)))
                {
                    return new List<Menu>();
                }
                return menu.Submenus;
            }
        }

        public int ItemsCount
        {
            get { return Math.Max(MatchingSubmenus.Count, MatchingExecutableItems.Count); }
        }

        public string StackText
        {
            get
            {
                List<Menu> items = _menusStack.Reverse().Skip(1).ToList();
                return "> " + string.Join(" > ", items.Select(x => x.Name));
            }
        }

        public Menu ActingMenu
        {
            get
            {
                if (_menusStack.Count < 2)
                {
                    return null;
                }

                Menu menu = _menusStack.Peek();
                if (_menusStack.Skip(1).Any(x => menu.Submenus.Any(y => y.Id == x.Id)))
                {
                    return menu;
                }

                return _menusStack.ToArray()[_menusStack.Count - 2];
            }
        }

        public void PushSelectedSubmenu()
        {
            Menu menu = MatchingSubmenus[SelectedIndex];
            _menusStack.Push(menu);
        }

        public void Clear()
        {
            while (_menusStack.Count > 1)
            {
                _menusStack.Pop();
            }
        }

        private bool MatchesFilter(string text)
        {
            string[] words = Filter.ToLower().Split(' ');
            return words.All(x => text.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public ExecutableItem GetExecutableItem()
        {
            List<ExecutableItem> executableItems = MatchingExecutableItems;
            if (executableItems.Count <= SelectedIndex)
            {
                return null;
            }
            return executableItems[SelectedIndex];
        }

        public void PopMenu()
        {
            if (_menusStack.Count > 1)
            {
                _menusStack.Pop();
            }
        }

        public void PersistExecutionTimeStamps()
        {
            DateTime now = DateTime.Now;

            foreach (Menu menu in _menusStack)
            {
                menu.LastAccess = now;
            }

            ExecutableItem executableItem = GetExecutableItem();
            if (executableItem != null)
            {
                executableItem.LastAccess = now;
            }

            var storage = new MenuStorage(Configuration.MenusFileName);
            Menu rootMenu = _menusStack.Reverse().First();

            storage.SaveMenus(rootMenu.Submenus);

            storage = new MenuStorage(ExecutableMenu.ContentsFileName);
            storage.SaveExecutableItems(ExecutableItems);
        }
    }
}