using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Ahk
{
    public class MenuEngine
    {
        private readonly MainForm _form;
        private readonly MenuCollection _menuCollection;
        private readonly Stack<Menu> _stack;
        private readonly Timer _textChangedTimer;
        private string _lastSearchText;

        public MenuEngine(MainForm form, MenuCollection menuCollection)
        {
            _form = form;
            _menuCollection = menuCollection;

            _stack = new Stack<Menu>();

            _textChangedTimer = new Timer();
            _textChangedTimer.Elapsed += (s, e) =>  OnTextChangedTimerElapsed();
            _textChangedTimer.Interval = 500;

            SearchBar.TextChanged += (s, e) => OnTextBoxTextChanged();
            SearchBar.KeyDown += OnTextBoxOnKeyDown;

            ClearSearchBar();
            LoadItems();
        }

        private string Filter { get; set; }

        private ListBox ListBox => _form.ListBox;

        private TextBox SearchBar => _form.TextBox;

        private Label StackLabel => _form.StackLabel;

        private List<Menu> Items
        {
            get
            {
                List<Menu> items;

                if (_stack.Count == 0)
                {
                    items = _menuCollection.Menus;
                }
                else
                {
                    items = _stack.Peek().SubItems;
                }

                return items
                    .Where(x => MatchesFilter(x.Name))
                    .OrderBy(x => x, new MenuItemComparator())
                    .ToList();
            }
        }

        private bool MatchesFilter(string text)
        {
            return text.IndexOf(Filter, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private List<ExecutableItem> ExecutableItems
        {
            get
            {
                if (_stack.Count == 0)
                {
                    return new List<ExecutableItem>();
                }

                var actionMenu = _stack.Peek() as ExecutableMenu;
                if (actionMenu == null)
                {
                    return new List<ExecutableItem>();
                }

                return actionMenu.ExecutableItems
                    .Where(x => MatchesFilter(x.Name))
                    .ToList();
            }
        }

        private void OnTextBoxOnKeyDown(object sender, KeyEventArgs args)
        {
            bool handled = false;

            if (args.KeyCode == Keys.Space)
            {
                handled = OnSpaceKeyPressed();
            }

            if (handled)
            {
                args.SuppressKeyPress = true;
                args.Handled = true;
            }
        }

        private bool OnSpaceKeyPressed()
        {
            if (Items.Count == 0 && !IsExecutableMenu)
            {
                return true;
            }

            Menu selectedMenu = Items[0];
            if (selectedMenu.SubItems.Count == 0)
            {
                var executableMenu = selectedMenu as ExecutableMenu;
                if (executableMenu != null)
                {
                    PushExecutableMenu(executableMenu);
                }
                return true;
            }

            PushMenu(selectedMenu);
            return true;
        }

        public bool IsExecutableMenu
        {
            get
            {
                if (_stack.Count == 0)
                {
                    return false;
                }
                return _stack.Peek() is ExecutableMenu;
            }
        }

        private void PushExecutableMenu(ExecutableMenu menu)
        {
            _textChangedTimer.Stop();
            _stack.Push(menu);
            ClearSearchBar();
            ReloadStackLabel();
            LoadExecutableItems();
            _textChangedTimer.Start();
        }

        private void PushMenu(Menu menu)
        {
            _textChangedTimer.Stop();
            _stack.Push(menu);
            ClearSearchBar();
            ReloadStackLabel();
            LoadItems();
            _textChangedTimer.Start();
        }

        private void ClearSearchBar()
        {
            Filter = string.Empty;
            SearchBar.Clear();
            _lastSearchText = SearchBar.Text;
        }

        private void ReloadStackLabel()
        {
            StackLabel.Text = "> " + string.Join(" > ", _stack.Select(x => x.Name));
        }

        private void OnTextBoxTextChanged()
        {
            _textChangedTimer.Stop();
            _textChangedTimer.Start();
        }

        private void LoadItems()
        {
            ListBox.Items.Clear();
            foreach (string name in Items.Select(x => x.Name))
            {
                ListBox.Items.Add(name);
            }
        }

        private void LoadExecutableItems()
        {
            ListBox.Items.Clear();
            foreach (string name in ExecutableItems.Select(x => x.Name))
            {
                ListBox.Items.Add(name);
            }
        }

        private void OnTextChangedTimerElapsed()
        {
            _textChangedTimer.Stop();

            if (_lastSearchText == SearchBar.Text)
            {
                return;
            }

            _lastSearchText = SearchBar.Text;

            SearchBar.InvokeCommand(OnFilterChanged);
        }

        private void OnFilterChanged()
        {
            Filter = SearchBar.Text.Trim();
            if (IsExecutableMenu)
            {
                LoadExecutableItems();
            }
            else
            {
                LoadItems();
            }
        }
    }
}