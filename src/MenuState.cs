using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class MenuState
    {
        private readonly Stack<Menu> _menuStack;

        public MenuState(Menu rootMenu)
        {
            _menuStack = new Stack<Menu>();
            _menuStack.Push(rootMenu);
        }

        public Menu RootMenu
        {
            get { return _menuStack.Reverse().First(); }
        }

        public List<BaseItem> MatchingItems
        {
            get
            {
                List<BaseItem> result = _menuStack.Peek().Items.Where(x => MatchesFilter(x.Name)).ToList();
                result.Sort(new MenuComparer(this));
                return result;
            }
        }

        public string Filter { get; set; }

        public bool IsMenuSelected
        {
            get { return SelectedMenu != null; }
        }

        public Menu SelectedMenu
        {
            get { return SelectedItem as Menu; }
        }

        public ExecutableItem SelectedExecutableItem
        {
            get { return SelectedItem as ExecutableItem; }
        }

        private BaseItem SelectedItem
        {
            get
            {
                if (SelectedIndex >= MatchingItems.Count)
                {
                    return null;
                }
                return MatchingItems[SelectedIndex];
            }
        }

        public bool IsExecutableItemSelected
        {
            get { return SelectedItem is ExecutableItem; }
        }

        public int SelectedIndex { get; set; }

        public Menu Menu
        {
            get { return _menuStack.Peek(); }
        }

        public int ItemsCount
        {
            get { return MatchingItems.Count; }
        }

        public string StackText
        {
            get
            {
                List<Menu> items = _menuStack.Reverse().Skip(1).ToList();
                return "> " + string.Join(" > ", items.Select(x => x.Name));
            }
        }

        public Menu ActingMenu
        {
            get
            {
                if (_menuStack.Count < 2)
                {
                    return null;
                }

                return _menuStack.Peek();
            }
        }

        public string Context { get; set; }

        public void PushSelectedSubmenu()
        {
            Menu menu = SelectedMenu;
            _menuStack.Push(menu);
            menu.LoadItems();
        }

        public void Clear()
        {
            while (_menuStack.Count > 1)
            {
                _menuStack.Pop();
            }
        }

        private bool MatchesFilter(string text)
        {
            string[] words = Filter.ToLower().Split(' ');
            return words.All(x => text.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public void PopMenu()
        {
            if (_menuStack.Count > 1)
            {
                _menuStack.Pop();
            }
        }

        public void PersistExecutionTimeStamps()
        {
            ReloadGuard.Enabled = false;

            DateTime now = DateTime.Now;

            foreach (Menu menu in _menuStack.Reverse().Skip(1).Reverse())
            {
                ExecutionTimeStamps.Instance.SetTimeStamp(menu.Id, now);
                now = now.AddTicks(1);
            }

            ExecutableItem executableItem = SelectedExecutableItem;
            if (executableItem != null)
            {
                ExecutionTimeStamps.Instance.SetTimeStamp(executableItem.Id, now);
            }

            ExecutionTimeStamps.Instance.Save();

            ReloadGuard.Enabled = true;
        }
    }
}