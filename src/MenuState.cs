using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutomationEngine
{
    public class MenuState
    {
        private readonly Stack<Menu> _menuStack;
        private string _filter;
        private List<BaseItem> _matchingItems;

        public MenuState(Menu rootMenu)
        {
            _menuStack = new Stack<Menu>();
            _menuStack.Push(rootMenu);
            _matchingItems = new List<BaseItem>();
        }

        public Menu RootMenu
        {
            get { return _menuStack.Reverse().First(); }
        }

        public bool IsRootMenuActive
        {
            get { return ActiveMenu == RootMenu; }
        }

        public List<BaseItem> MatchingItems
        {
            get { return _matchingItems; }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                FilterWords = Filter.ToLower().Split(' ');
                DetermineMatchingItems();
            }
        }

        private string[] FilterWords { get;set; }

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
            get { return MatchingItems.ElementAtOrDefault(SelectedIndex); }
        }

        public bool IsExecutableItemSelected
        {
            get { return SelectedItem is ExecutableItem; }
        }

        public int SelectedIndex { get; set; }

        public int ItemsCount
        {
            get { return MatchingItems.Count; }
        }

        public string StackText
        {
            get
            {
                List<Menu> items = _menuStack.Reverse().Skip(1).ToList();
                return "> " + string.Join(" > ", items.Select(x => x.GetProperName()));
            }
        }

        public Menu ActiveMenu
        {
            get { return _menuStack.Peek(); }
        }

        public string Context { get; set; }

        public bool SinglePushMenu
        {
            get
            {
                if (!IsMenuSelected)
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Filter))
                {
                    return false;
                }
                return MatchingItems.Count(x => x is Menu && x.IsPerfectMatch) == 1;
            }
        }

        private void DetermineMatchingItems()
        {
            IEnumerable<BaseItem> selectableItems = ActiveMenu.GetSelectableItems();
            _matchingItems = selectableItems.Where(MatchesFilter).ToList();
            _matchingItems.Sort(new MenuComparer(this));
        }

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

        private bool MatchesFilter(BaseItem item)
        {
            if (Filter.Trim().Length == 0)
            {
                return true;
            }

            if (item.NamePatternsSpecified)
            {
                return MatchesPattern(item);
            }

            string[] itemWords = item.Name.ToLower().Split(' ');
            return FilterWords.All(x => itemWords.Any(y => y.StartsWith(x, StringComparison.InvariantCultureIgnoreCase)));
        }

        private bool MatchesPattern(BaseItem item)
        {
            var matchEvaluator = new FilterMatchEvaluator(item, FilterWords)
            {
                CheckForPerfectMatch = _menuStack.Count == 1
            };
            matchEvaluator.Evaluate();
            return matchEvaluator.MatchesFilter;
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

            UpdateMenuExecutionTimeStamps();
            UpdateItemExecutionTimeStamp();
            ExecutionTimeStamps.Instance.Save();

            ReloadGuard.Enabled = true;
        }

        private void UpdateItemExecutionTimeStamp()
        {
            ExecutableItem executableItem = SelectedExecutableItem;

            if (executableItem == null)
            {
                return;
            }

            DateTime now = DateTime.Now;

            executableItem.LastAccess = now;
            if (executableItem.ReplacedItemId == null)
            {
                ExecutionTimeStamps.Instance.SetTimeStamp(executableItem.Id, now);
                return;
            }

            ExecutableItem replacedExecutableItem = ActiveMenu.Items
                .OfType<ExecutableItem>()
                .First(x => x.Id == executableItem.ReplacedItemId);

            replacedExecutableItem.LastAccess = now;
            ExecutionTimeStamps.Instance.SetTimeStamp(replacedExecutableItem.Id, now);
        }

        private void UpdateMenuExecutionTimeStamps()
        {
            DateTime now = DateTime.Now;
            foreach (Menu menu in _menuStack.Reverse().Skip(1).Reverse())
            {
                menu.LastAccess = now;
                ExecutionTimeStamps.Instance.SetTimeStamp(menu.Id, now);
                now = now.AddTicks(1);
            }
        }

        public void SetCurrentContext()
        {
            ExecutableItem executableItem = SelectedExecutableItem;

            string newContext = null;
            if (executableItem != null && executableItem.Context != null)
            {
                newContext = executableItem.Context;
            }
            else
            {
                newContext = _menuStack
                    .Where(x => x.ContextSpecified)
                    .Select(x => x.Context)
                    .LastOrDefault();
            }

            if (newContext != null)
            {
                ContextCollection.Instance.Current = newContext;
            }
        }
    }
}