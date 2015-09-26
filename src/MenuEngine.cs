using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Ahk
{
    public class MenuEngine
    {
        private readonly MenuCollection _menuCollection;
        private readonly Stack<Menu> _stack;
        private readonly Timer _textChangedTimer;
        private string _lastSearchText;

        public MenuEngine(MainForm form, MenuCollection menuCollection)
        {
            Form = form;
            _menuCollection = menuCollection;

            _stack = new Stack<Menu>();

            _textChangedTimer = new Timer();
            _textChangedTimer.Elapsed += (s, e) =>  OnTextChangedTimerElapsed();
            _textChangedTimer.Interval = 500;

            SearchBar.TextChanged += (s, e) => OnTextBoxTextChanged();
            SearchBar.KeyDown += OnTextBoxOnKeyDown;

            ClearSearchBar();
        }

        private string Filter { get; set; }

        private MainForm Form { get; }

        private ListBox ListBox => Form.ListBox;

        private TextBox SearchBar => Form.TextBox;

        private Label StackLabel => Form.StackLabel;

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
            string[] words = Filter.ToLower().Split(' ');
            return words.All(x => text.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);
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

            if (args.KeyCode == Keys.Enter)
            {
                handled = OnEnterKeyPressed();
            }

            if (args.KeyCode == Keys.Down)
            {
                SelectItem(ListBox.SelectedIndex + 1);
                handled = true;
            }

            if (args.KeyCode == Keys.Up)
            {
                SelectItem(ListBox.SelectedIndex - 1);
                handled = true;
            }

            if (args.KeyCode == Keys.Escape)
            {
                CloseMenuEngine();
                handled = true;
            }

            if (args.KeyData == (Keys.Control | Keys.Back))
            {
                SendKeys.SendWait("^+{LEFT}{BACKSPACE}");
                handled = true;
            }
            else if (args.KeyData == (Keys.Alt | Keys.Back))
            {
                if (SearchBar.Text == string.Empty)
                {
                    PopMenu();
                }
                else
                {
                    ClearSearchBar();
                }
                handled = true;
            }

            if (handled)
            {
                args.SuppressKeyPress = true;
                args.Handled = true;
            }
        }

        private bool OnEnterKeyPressed()
        {
            if (_textChangedTimer.Enabled)
            {
                _textChangedTimer.Stop();
                OnFilterChanged();
            }

            if (IsExecutableMenu && ExecutableItems.Count > 0)
            {
                ExecuteItem(ExecutableItems[0]);
            }
            else if (!IsExecutableMenu && Items.Count > 0)
            {
                PushMenu(Items[0]);
            }
            return true;
        }

        private void ExecuteItem(ExecutableItem executableItem)
        {
            var menu = (ExecutableMenu)_stack.Peek();
            menu.Execute(executableItem);
            CloseMenuEngine();
        }

        private void CloseMenuEngine()
        {
            ResetMenuEngine();
            Form.Visible = false;
        }

        private void ResetMenuEngine()
        {
            _stack.Clear();
            ClearSearchBar();
        }

        private bool OnSpaceKeyPressed()
        {
            if (IsExecutableMenu)
            {
                return false;
            }

            return ProcessSelectedItemIfAny();
        }

        private bool ProcessSelectedItemIfAny()
        {
            if (_textChangedTimer.Enabled)
            {
                _textChangedTimer.Stop();
                OnFilterChanged();
            }

            if (Items.Count == 0)
            {
                return true;
            }

            var executableMenu = Items[0] as ExecutableMenu;
            if (executableMenu != null)
            {
                PushExecutableMenu(executableMenu);
            }
            else
            {
                PushMenu(Items[0]);
            }

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
            _stack.Push(menu);
            _textChangedTimer.Stop();
            ClearSearchBar();
            _textChangedTimer.Start();
        }

        private void PopMenu()
        {
            if (_stack.Count == 0)
            {
                return;
            }

            _stack.Pop();
            _textChangedTimer.Stop();
            ClearSearchBar();
            _textChangedTimer.Start();
        }

        private void PushMenu(Menu menu)
        {
            _stack.Push(menu);
            _textChangedTimer.Stop();
            ClearSearchBar();
            _textChangedTimer.Start();
        }

        private void ClearSearchBar()
        {
            Filter = string.Empty;
            SearchBar.Clear();
            _lastSearchText = SearchBar.Text;
            ReloadStackLabel();
            LoadProperItems();
        }

        private void SelectItem(int itemIndex)
        {
            if (itemIndex < 0)
            {
                return;
            }
            if (Items.Count > itemIndex || ExecutableItems.Count > itemIndex)
            {
                ListBox.SelectedIndex = itemIndex;
            }
        }

        private void ReloadStackLabel()
        {
            StackLabel.Text = "> " + string.Join(" > ", _stack.Reverse().Select(x => x.Name));
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
            LoadProperItems();
        }

        private void LoadProperItems()
        {
            if (IsExecutableMenu)
            {
                LoadExecutableItems();
            }
            else
            {
                LoadItems();
            }

            SelectItem(0);
        }
    }
}