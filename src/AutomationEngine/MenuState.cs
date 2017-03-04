using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomationEngine
{
    public class MenuState
    {
        private readonly Stack<Menu> _menuStack;

        private string _filter;
        private List<BaseItem> _matchingItems;
        private string _applicationContext;
        private string _alternateRootMenuAlias;
        private BaseItem _itemWithOpenedContextMenu;

        public MenuState(Menu rootMenu)
        {
            RootMenu = rootMenu;

            _menuStack = new Stack<Menu>();
            _menuStack.Push(rootMenu);

            _matchingItems = new List<BaseItem>();
        }

        public Menu RootMenu { get; }

        public bool IsRootMenuActive => ActiveMenu == RootMenu;

        public List<BaseItem> MatchingItems => _matchingItems;

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

        public bool IsMenuSelected => SelectedMenu != null;

        public Menu SelectedMenu => SelectedItem as Menu;

        public ExecutableItem SelectedExecutableItem => SelectedItem as ExecutableItem;

        public BaseItem SelectedItem => MatchingItems.ElementAtOrDefault(SelectedIndex);

        public bool IsExecutableItemSelected => SelectedItem is ExecutableItem;

        public int SelectedIndex { get; set; }

        public int ItemsCount => MatchingItems.Count;

        public string StackText
        {
            get
            {
                List<Menu> items = _menuStack.Reverse().Skip(1).ToList();
                return "> " + string.Join(" > ", items.Select(x => x.GetProperName()));
            }
        }

        public Menu ActiveMenu => _menuStack.Peek();

        public string ApplicationContext
        {
            get { return _applicationContext; }
            set
            {
                _applicationContext = value;
                PrepareApplicationMenuStack();
            }
        }

        public string AlternateRootMenuAlias
        {
            get { return _alternateRootMenuAlias; }
            set
            {
                _alternateRootMenuAlias = value;
                PrepareAlternateRootMenu();
            }
        }

        public bool IncludeArchivedItems { get; set; }

        public BaseItem ItemWithOpenedContextMenu
        {
            get { return _itemWithOpenedContextMenu; }
            set
            {
                _itemWithOpenedContextMenu = value;
                PrepareContextMenuStack();
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
                if (!IsRootMenuActive)
                {
                    SetRootMenu(RootMenu);
                }
                return;
            }

            ApplicationMenuFileContext applicationMenuFileContext = GetApplicationMenuByContext(ApplicationContext);
            if (applicationMenuFileContext == null)
            {
                SetRootMenu(ApplicationMenu.DefaultApplicationMenu);
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

            SetRootMenu(menu);
        }

        private void PrepareAlternateRootMenu()
        {
            if (AlternateRootMenuAlias == null)
            {
                if (!IsRootMenuActive)
                {
                    SetRootMenu(RootMenu);
                }
            }
            else
            {
                Menu menu = RootMenu.FindMenuByAlias(AlternateRootMenuAlias);
                SetRootMenu(menu);
            }
        }

        private void PrepareContextMenuStack()
        {
            if (ItemWithOpenedContextMenu == null)
            {
                if (!IsRootMenuActive)
                {
                    SetRootMenu(RootMenu);
                }
            }
            else
            {
                Menu menu = ContextMenuCollection.Instance.GetItemContextMenu();
                SetRootMenu(menu);
            }
        }

        private void SetRootMenu(Menu menu)
        {
            _menuStack.Clear();
            _menuStack.Push(menu);
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
            UpdateMenuExecutionTimeStamps();
            UpdateItemExecutionTimeStamp();
            ExecutionTimeStamps.Instance.Save();
        }

        private void UpdateItemExecutionTimeStamp()
        {
            ExecutableItem executableItem = SelectedExecutableItem;

            if (string.IsNullOrWhiteSpace(executableItem?.Id))
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

            string newContext;
            if (executableItem?.Context != null)
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