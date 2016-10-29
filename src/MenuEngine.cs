﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace AutomationEngine
{
    public class MenuEngine
    {
        private static MenuEngine _engine;

        public static MenuEngine Instance => _engine;

        private readonly Timer _textChangedTimer;

        private string _lastSearchText;

        private bool _selectedIndexChangedEventDeactivated;

        public MenuEngine(MainForm form, Menu rootMenu)
        {
            Form = form;
            Form.Execute += OnExecute;
            Form.ShortcutPressed += OnShortcutPressed;
            State = new MenuState(rootMenu);

            _textChangedTimer = new Timer();
            _textChangedTimer.Elapsed += (s, e) =>  OnTextChangedTimerElapsed();
            _textChangedTimer.Interval = 100;

            SearchBar.TextChanged += (s, e) => OnTextBoxTextChanged();
            SearchBar.KeyDown += OnTextBoxKeyDown;
            ListBox.SelectedIndexChanged += (s, a) => OnSelectedIndexChanged();

            ClearSearchBar();
        }

        private MenuState State { get; set; }

        private MainForm Form { get; }

        public bool WorkInProgressVisible
        {
            get { return Form.InvokeQuery(() => Form.WorkInProgressVisible); }
            set { Form.InvokeCommand(() => Form.WorkInProgressVisible = value); }
        }

        private ListBox ListBox => Form.ListBox;

        private TextBox SearchBar => Form.TextBox;

        private Label StackLabel => Form.StackLabel;

        public string ApplicationContext
        {
            get { return State.ApplicationContext; }
            set { State.ApplicationContext = value; }
        }

        private void OnSelectedIndexChanged()
        {
            if (!_selectedIndexChangedEventDeactivated)
            {
                SelectItem(ListBox.SelectedIndex);
            }
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs args)
        {
            bool handled = false;

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

        private void OnExecute()
        {
            if (_textChangedTimer.Enabled)
            {
                _textChangedTimer.Stop();
                OnFilterChanged();
            }

            if (State.IsMenuSelected)
            {
                PushSelectedSubmenu();
            }
            else if (State.IsExecutableItemSelected)
            {
                ExecuteSelectedItem();
            }
        }

        private void OnShortcutPressed(ActionType actionType)
        {
            if (actionType == ActionType.ToggleArchiveSearch && ApplicationContext == null)
            {
                State.IncludeArchivedItems = !State.IncludeArchivedItems;
                UpdateStateLabel();
                OnFilterChanged();
            }
            if (actionType == ActionType.OpenContextMenuForSelectedItem)
            {
            }
            if (actionType == ActionType.DeleteMenuEntry)
            {
                new MenuEntryDeletion(State).Delete();
            }
        }

        private void UpdateStateLabel()
        {
            var text = new StringBuilder();

            if (State.ApplicationContext == null)
            {
                text.Append(State.IncludeArchivedItems ? "ARCHIVE," : string.Empty);
            }
            else
            {
                text.Append("APPLICATION MENU");
            }

            Form.StateLabel.Text = text.ToString().TrimEnd(',');
            Form.StateLabel.Visible = Form.StateLabel.Text.Length > 0;
        }

        private void ExecuteSelectedItem()
        {
            ExecutableItem executableItem = State.SelectedExecutableItem;
            if (executableItem == null)
            {
                return;
            }

            string executingMethodName = null;
            if (executableItem.ExecutingMethodName != null)
            {
                executingMethodName = executableItem.ExecutingMethodName;
            }
            else if (State.ActiveMenu != null)
            {
                executingMethodName = State.ActiveMenu.ExecutingMethodName;
            }

            if (!executableItem.ActionTypeSpecified && executingMethodName == null)
            {
                return;
            }

            State.PersistExecutionTimeStamps();

            State.SetCurrentContext();

            CloseMenuEngine();

            if (executableItem.ActionTypeSpecified)
            {
                OnExecutingManagedAction(executableItem);
            }
            else
            {
                List<AbstractValue> arguments = GetExecutableItemArguments(executableItem);
                ExecuteAhkMethod(executingMethodName, arguments);
            }
        }

        private void OnExecutingManagedAction(ExecutableItem executableItem)
        {
            ActionType actionType = executableItem.ActionType;

            if (actionType == ActionType.AddNewMenuEntry)
            {
                FormFactory.Instance<AddFileItemForm>().Show();
            }
            if (actionType == ActionType.CreateApplicationMenu)
            {
                var createApplicationMenuForm = FormFactory.Instance<CreateApplicationMenuForm>();
                createApplicationMenuForm.ContextRegex = ApplicationContext;
                createApplicationMenuForm.Show();
            }
        }

        private List<AbstractValue> GetExecutableItemArguments(ExecutableItem executableItem)
        {
            if (executableItem is FileItem)
            {
                return new List<AbstractValue>
                {
                    new StringValue { Value = ((FileItem)executableItem).FilePath }
                };
            }
            return executableItem.Arguments;
        }

        private void ExecuteAhkMethod(string evaluateResultMethod, List<AbstractValue> arguments)
        {
            AhkInterop.ExecuteMethod(evaluateResultMethod, arguments.ToArray());
        }

        private void CloseMenuEngine()
        {
            Form.Visible = false;
        }

        public void ResetMenuEngine()
        {
            State.IncludeArchivedItems = ApplicationContext != null;
            UpdateStateLabel();

            if (!State.IsRootMenuActive || SearchBar.Text.Length > 0)
            {
                State.Clear();
                ClearSearchBar();
            }
        }

        private void PushSelectedSubmenu()
        {
            WorkInProgressVisible = true;

            using (var backgroundWorker = new BackgroundWorker())
            {
                backgroundWorker.DoWork += (sender, args) =>
                {
                    State.PushSelectedSubmenu();
                    ClearSearchBar();
                };
                backgroundWorker.RunWorkerCompleted += (sender, args) =>
                {
                    WorkInProgressVisible = false;
                };
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void PopMenu()
        {
            State.PopMenu();
            ClearSearchBar();
        }

        public void ClearSearchBar()
        {
            _textChangedTimer.Stop();
            Form.InvokeCommand(() =>
            {
                State.Filter = string.Empty;
                SearchBar.Clear();
                _lastSearchText = SearchBar.Text;
                ReloadStackLabel();
                LoadItems();
            });
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
                _selectedIndexChangedEventDeactivated = true;
                ListBox.SelectedIndex = itemIndex;
                _selectedIndexChangedEventDeactivated = false;
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
            LoadItems();
        }

        private void LoadItems()
        {
            ListBox.Items.Clear();
            foreach (BaseItem item in State.MatchingItems)
            {
                ListBox.Items.Add(item.GetProperName());
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
