using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Ahk
{
    public class MenuEngine
    {
        private static MenuEngine _engine;

        private readonly Timer _textChangedTimer;

        private string _lastSearchText;

        public MenuEngine(MainForm form, Menu rootMenu)
        {
            Form = form;
            State = new MenuState(rootMenu);

            _textChangedTimer = new Timer();
            _textChangedTimer.Elapsed += (s, e) =>  OnTextChangedTimerElapsed();
            _textChangedTimer.Interval = 100;

            SearchBar.TextChanged += (s, e) => OnTextBoxTextChanged();
            SearchBar.KeyDown += OnTextBoxKeyDown;

            ClearSearchBar();
        }

        private MenuState State { get; set; }

        private MainForm Form { get; }

        private ListBox ListBox => Form.ListBox;

        private TextBox SearchBar => Form.TextBox;

        private Label StackLabel => Form.StackLabel;

        private void OnTextBoxKeyDown(object sender, KeyEventArgs args)
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

            if (State.IsExecutableItemSelected)
            {
                ExecuteSelectedItem();
            }
            else if (State.IsSubmenuSelected)
            {
                State.PushSelectedSubmenu();
                ClearSearchBar();
            }
            return true;
        }

        private void ExecuteSelectedItem()
        {
            ExecutableItem executableItem = State.GetExecutableItem();
            if (executableItem == null)
            {
                return;
            }

            Menu actingMenu = State.ActingMenu;
            if (actingMenu == null)
            {
                return;
            }

            State.PersistExecutionTimeStamps();

            CloseMenuEngine();

            ExecuteFunction(actingMenu.ExecutingMethodName, executableItem.Arguments);
        }

        private void ExecuteFunction(string evaluateResultMethod, List<ExecutableItemArgument> arguments)
        {
            string[] values = arguments.Select(x =>
            {
                if (x.Type == ArgumentType.String)
                {
                    return "\"" + x.Value + "\"";
                }
                return x.Value;
            }).ToArray();
            AhkInterop.ExecFunction(evaluateResultMethod, values);
        }

        private void CloseMenuEngine()
        {
            ResetMenuEngine();
            Form.Visible = false;
        }

        private void ResetMenuEngine()
        {
            State.Clear();
            ClearSearchBar();
        }

        private bool OnSpaceKeyPressed()
        {
            if (State.IsSelectionMenu)
            {
                return false;
            }

            if (_textChangedTimer.Enabled)
            {
                _textChangedTimer.Stop();
                OnFilterChanged();
            }

            if (State.IsSubmenuSelected)
            {
                State.PushSelectedSubmenu();
                ClearSearchBar();
            }

            return true;
        }

        private void PopMenu()
        {
            State.PopMenu();
            ClearSearchBar();
        }

        private void ClearSearchBar()
        {
            _textChangedTimer.Stop();
            State.Filter = string.Empty;
            SearchBar.Clear();
            _lastSearchText = SearchBar.Text;
            ReloadStackLabel();
            LoadProperItems();
            _textChangedTimer.Start();
        }

        private void SelectItem(int itemIndex)
        {
            if (itemIndex < 0)
            {
                return;
            }

            if (State.ItemsCount > itemIndex)
            {
                ListBox.SelectedIndex = itemIndex;
                State.SelectedIndex = itemIndex;
            }
        }

        private void ReloadStackLabel()
        {
            StackLabel.Text = State.StackText;
        }

        private void OnTextBoxTextChanged()
        {
            _textChangedTimer.Stop();
            _textChangedTimer.Start();
        }

        private void LoadItems()
        {
            ListBox.Items.Clear();
            foreach (string name in State.MatchingSubmenus.Select(x => x.Name))
            {
                ListBox.Items.Add(name);
            }
        }

        private void LoadExecutableItems()
        {
            ListBox.Items.Clear();
            foreach (string name in State.MatchingExecutableItems.Select(x => x.Name))
            {
                ListBox.Items.Add(name);
            }
        }

        private void OnTextChangedTimerElapsed()
        {
            _textChangedTimer.Stop();

            string currentValue = SearchBar.InvokeQuery(() => SearchBar.Text);
            if (_lastSearchText !=  currentValue)
            {
                SearchBar.InvokeCommand(OnFilterChanged);
            }
        }

        private void OnFilterChanged()
        {
            _lastSearchText = SearchBar.Text;
            State.Filter = SearchBar.Text.Trim();
            LoadProperItems();
        }

        private void LoadProperItems()
        {
            if (State.IsSelectionMenu)
            {
                LoadExecutableItems();
            }
            else
            {
                LoadItems();
            }

            SelectItem(0);
        }

        public static void Start(MainForm mainForm, Menu rootMenu)
        {
            if (_engine == null)
            {
                _engine = new MenuEngine(mainForm, rootMenu);
            }

            _engine.State = new MenuState(rootMenu);
            _engine.ClearSearchBar();
        }
    }
}
