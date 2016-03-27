using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationEngine
{
    public class MenuState
    {
        private readonly Stack<Menu> _itemStack;

        public MenuState(Menu rootMenu)
        {
            _itemStack = new Stack<Menu>();
            _itemStack.Push(rootMenu);
        }

        public Menu RootMenu
        {
            get { return _itemStack.Reverse().First(); }
        }

        public List<BaseItem> MatchingItems
        {
            get
            {
                List<BaseItem> result = _itemStack.Peek().Items.Where(x => MatchesFilter(x.Name)).ToList();
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
            get { return _itemStack.Peek(); }
        }

        public int ItemsCount
        {
            get { return MatchingItems.Count; }
        }

        public string StackText
        {
            get
            {
                List<Menu> items = _itemStack.Reverse().Skip(1).ToList();
                return "> " + string.Join(" > ", items.Select(x => x.Name));
            }
        }

        public Menu ActingMenu
        {
            get
            {
                if (_itemStack.Count < 2)
                {
                    return null;
                }

                return _itemStack.Peek();
            }
        }

        public string Context { get; set; }

        public void PushSelectedSubmenu()
        {
            Menu menu = SelectedMenu;
            _itemStack.Push(menu);
            menu.LoadItems();
        }

        public void Clear()
        {
            while (_itemStack.Count > 1)
            {
                _itemStack.Pop();
            }
        }

        private bool MatchesFilter(string text)
        {
            string[] words = Filter.ToLower().Split(' ');
            return words.All(x => text.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public void PopMenu()
        {
            if (_itemStack.Count > 1)
            {
                _itemStack.Pop();
            }
        }

        public void PersistExecutionTimeStamps()
        {
        }
    }
}