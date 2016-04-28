using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomationEngine
{
    public class MenuState
    {
        private readonly Stack<Menu> _menuStack;
        private readonly Stack<Menu> _applicationMenuStack;
        private string _filter;
        private List<BaseItem> _matchingItems;
        private bool _includeArchivedItems;
        private string _applicationContext;

        public MenuState(Menu rootMenu)
        {
            _menuStack = new Stack<Menu>();
            _menuStack.Push(rootMenu);

            _applicationMenuStack = new Stack<Menu>();

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
                FilterWords = Filter.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
            get
            {
                if (_applicationMenuStack.Count > 0)
                {
                    return _applicationMenuStack.Peek();
                }
                return _menuStack.Peek();
            }
        }

        public string ApplicationContext
        {
            get { return _applicationContext; }
            set
            {
                _applicationContext = value;
                PrepareApplicationMenuStack();
            }
        }

        public bool IncludeArchivedItems
        {
            get { return _includeArchivedItems; }
            set
            {
                _includeArchivedItems = value;
                DetermineMatchingItems();
            }
        }

        private void DetermineMatchingItems()
        {
            IEnumerable<BaseItem> selectableItems = ActiveMenu.GetSelectableItems();
            if (!IncludeArchivedItems)
            {
                int days = Configuration.Instance.ArchiveDayCountThreshold;
                DateTime dateTime = DateTime.Now.Date.AddDays(-days);
                selectableItems = selectableItems.Where(x => x.LastAccess >= dateTime);
            }
            _matchingItems = selectableItems.Where(MatchesFilter).ToList();
            _matchingItems.Sort(new MenuComparer(this));
        }

        private void PrepareApplicationMenuStack()
        {
            if (ApplicationContext == null)
            {
                _applicationMenuStack.Clear();
                return;
            }

            ApplicationMenuFileContext applicationMenuFileContext = GetApplicationMenuByContext(ApplicationContext);
            if (applicationMenuFileContext == null)
            {
                _applicationMenuStack.Push(ApplicationMenu.DefaultApplicationMenu);
                return;
            }

            string fileName = applicationMenuFileContext.MenuFileName;
            if (!fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".xml";
            }

            string directory = Configuration.Instance.ApplicationMenuDirectory;
            string filePath = Path.Combine(directory, fileName);

            ApplicationMenu menu = Menu.LoadFromFile<ApplicationMenu>(filePath);

            if (!File.Exists(filePath))
            {
                menu.SaveToFile();
            }

            _applicationMenuStack.Push(menu);
        }

        private ApplicationMenuFileContext GetApplicationMenuByContext(string context)
        {
            return ApplicationMenuCollection.Instance.GetMenuByContext(context);
        }

        public void PushSelectedSubmenu()
        {
            SelectedMenu.LoadItemsIfNecessary();
            _menuStack.Push(SelectedMenu);
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
            if (!item.NameWordsSpecified)
            {
                return false;
            }

            return MatchesPattern(item);
        }

        private bool MatchesPattern(BaseItem item)
        {
            var matchEvaluator = new FilterMatchEvaluator(item, FilterWords);
            matchEvaluator.Evaluate();
            return item.MatchScore > 0;
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

            if (executableItem == null || string.IsNullOrWhiteSpace(executableItem.Id))
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

            ExecutableItem replacedExecutableItem = ActiveMenu
                .GetAllItems()
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
                Configuration.Instance.CurrentContext = newContext;
                Configuration.Instance.Save();
            }
        }
    }
}