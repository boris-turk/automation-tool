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
            get { return Submenus.Where(x => MatchesFilter(x.Name)).ToList(); }
        }

        public string Filter { get; set; }

        public bool IsSelectionMenu
        {
            get { return ExecutableItems.Count > 0; }
        }

        private List<ExecutableItem> ExecutableItems
        {
            get { return _menusStack.Peek().ExecutableItems; }
        }

        public List<ExecutableItem> MatchingExecutableItems
        {
            get { return ExecutableItems.Where(x => MatchesFilter(x.Name)).ToList(); }
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
            get { return _menusStack.Peek().Submenus; }
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
    }
}