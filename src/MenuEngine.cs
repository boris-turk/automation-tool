using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace AutomationEngine
{
    public class MenuEngine
    {
        private const string ExecuteAutomationEngineMethodIdentifier = "ExecuteAutomationEngineMethod";

        private static MenuEngine _engine;

        public static MenuEngine Instance
        {
            get { return _engine; }
        }

        private readonly Timer _textChangedTimer;

        private string _lastSearchText;

        private bool _selectedIndexChangedEventDeactivated;

        public MenuEngine(MainForm form, Menu rootMenu)
        {
            Form = form;
            Form.Execute += OnExecute;
            Form.ChildControlKeyDown += OnKeyDown;
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

        private MainForm Form { get; set; }

        public bool WorkInProgressVisible
        {
            get { return Form.InvokeQuery(() => Form.WorkInProgressVisible); }
            set { Form.InvokeCommand(() => Form.WorkInProgressVisible = value); }
        }

        private ListBox ListBox
        {
            get { return Form.ListBox; }
        }

        private TextBox SearchBar
        {
            get { return Form.TextBox; }
        }

        private Label StackLabel
        {
            get { return Form.StackLabel; }
        }

        public string Context
        {
            get { return State.Context; }
            set { State.Context = value; }
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

            if (args.KeyCode == Keys.Space)
            {
                //handled = OnSpaceKeyPressed();
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

        private void OnKeyDown(KeyEventArgs eventArgs)
        {
            var shortcutEventDispatcher = new ShortcutEventDispatcher(State, eventArgs);
            shortcutEventDispatcher.Dispatch();
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

            if (executingMethodName == null)
            {
                return;
            }

            State.PersistExecutionTimeStamps();

            State.SetCurrentContext();

            CloseMenuEngine();

            List<AbstractValue> arguments = GetExecutableItemArguments(executableItem);
            if (executingMethodName == ExecuteAutomationEngineMethodIdentifier)
            {
                ExecuteAutomationEngineMethod(arguments);
            }
            else
            {
                ExecuteAhkMethod(executingMethodName, arguments);
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

        private void ExecuteAutomationEngineMethod(List<AbstractValue> arguments)
        {
            var executor = new MenuEngineMethodExecutor(Form, arguments.Select(x => x.Value));
            executor.Execute();
        }

        private void ExecuteAhkMethod(string evaluateResultMethod, List<AbstractValue> arguments)
        {
            AhkInterop.ExecuteMethod(evaluateResultMethod, arguments.ToArray());
        }

        private void CloseMenuEngine()
        {
            Form.Visible = false;
            ResetMenuEngine();
        }

        private void ResetMenuEngine()
        {
            State.Clear();
            ClearSearchBar();
        }

        private bool OnSpaceKeyPressed()
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

            return true;
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
            foreach (string name in State.MatchingItems.Select(x => x.Name))
            {
                ListBox.Items.Add(name);
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
