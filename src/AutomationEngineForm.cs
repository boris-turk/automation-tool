using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutomationEngine.Messages;

namespace AutomationEngine
{
    public class AutomationEngineForm : Form
    {
        private const string AutomationEngineWindowTitle = "Automation engine";

        public event Action<ActionType> ShortcutPressed;

        public AutomationEngineForm()
        {
            KeyPreview = true;
            TopMost = true;
            StartPosition = FormStartPosition.CenterScreen;
            Load += (sender, args) => Initialize();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                OnCloseRequested();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (!e.Alt && !e.Control && e.KeyCode == Keys.Enter)
            {
                OnEnterKeyPressed();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
            {
                List<AutomationAction> actions = Configuration.Instance.Actions;

                AutomationAction shortcut = actions.FirstOrDefault(x =>
                    x.Shortcut.Alt == e.Alt &&
                    x.Shortcut.Control == e.Control &&
                    (x.Shortcut.Key == e.KeyCode || x.Shortcut.Character == e.KeyCode.ToCharacter()));

                if (shortcut != null)
                {
                    OnShortcutPressed(shortcut);
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        private void OnShortcutPressed(AutomationAction action)
        {
            ShortcutPressed?.Invoke(action.ActionType);
        }

        public bool Executed { get; private set; }

        protected bool ExecutionCanceled { get; set; }

        protected virtual string ValidateInput()
        {
            return null;
        }

        protected virtual string WindowName => string.Empty;

        protected List<Control> ChildControls => Controls.OfType<Control>().ToList();

        protected List<Control> FocusableControls
        {
            get
            {
                return ChildControls
                    .Where(x => x.TabStop)
                    .OrderBy(x => x.TabIndex)
                    .ToList();
            }
        }

        public Control FocusedControl
        {
            get { return ChildControls.FirstOrDefault(x => x.Focused); }
        }

        private Form VisibleChildForm
        {
            get
            {
                return Application.OpenForms.
                    OfType<Form>().
                    Where(x => x.Visible).
                    FirstOrDefault(x => x.GetType() != typeof(MainForm));
            }
        }

        private void Initialize()
        {
            ApplySelectAllTextOnFocusPolicy();
            SetWindowTitle();
        }

        private void ApplySelectAllTextOnFocusPolicy()
        {
            List<TextBox> textBoxes = FocusableControls.OfType<TextBox>().ToList();
            textBoxes.ForEach(control => control.GotFocus += (s, a) =>
            {
                control.SelectAll();
            });
        }

        private void SetWindowTitle()
        {
            if (string.IsNullOrWhiteSpace(WindowName))
            {
                Text = AutomationEngineWindowTitle;
            }
            else
            {
                Text = AutomationEngineWindowTitle + " - " + WindowName;
            }
        }

        private void OnEnterKeyPressed()
        {
            if (FocusedControl != FocusableControls.Last())
            {
                MoveFocusToNextInputControl();
                return;
            }

            string inputValidationErrorMessage = ValidateInput();
            if (inputValidationErrorMessage != null)
            {
                MessageBox.Show(inputValidationErrorMessage);
                return;
            }

            ExecutionCanceled = false;
            Executed = true;
            OnExecute();

            if (ExecutionCanceled)
            {
                Executed = false;
            }

            if (GetType() != typeof(MainForm) && !ExecutionCanceled)
            {
                Hide();
            }
        }

        private void MoveFocusToNextInputControl()
        {
            int focusedControlIndex = FocusableControls.IndexOf(FocusedControl);
            if (focusedControlIndex + 1 < FocusableControls.Count)
            {
                FocusableControls[focusedControlIndex + 1].Focus();
            }
        }

        protected virtual void OnExecute()
        {
        }

        private void OnCloseRequested()
        {
            Hide();
        }

        protected override void WndProc(ref Message m)
		{
		    if (m.Msg == WindowMessages.WmCopydata)
		    {
		        OnWmCopyData(m);

				return;
			}
            if (m.Msg == WindowMessages.WmSyscommand)
            {
                if (m.WParam.ToInt32() == WindowMessages.ScMinimize)
                {
                    m.Result = IntPtr.Zero;
                    Visible = false;
                    return;
                }
                if (m.WParam.ToInt32() == WindowMessages.ScMaximize)
                {
                    Visible = true;
                    return;
                }
            }
			base.WndProc(ref m);
		}

        protected override void OnShown(EventArgs e)
        {
            Executed = false;
            base.OnShown(e);
        }

        private void OnWmCopyData(Message message)
        {
            var mystr = new CopyDataStruct();
            Type mytype = mystr.GetType();
            mystr = (CopyDataStruct)message.GetLParam(mytype);

            if (mystr.LpData == WindowMessages.ToggleGLobalMenuVisibility)
            {
                MenuEngine.Instance.ApplicationContext = null;
                ToggleAutomationEngineVisibility();
            }
            else if (mystr.LpData == WindowMessages.ToggleContextMenuVisibility)
            {
                MenuEngine.Instance.ApplicationContext = AhkInterop.GetMessageFileContents().FirstOrDefault();
                ToggleAutomationEngineVisibility();
            }
            else if (mystr.LpData == WindowMessages.AhkFunctionResultReported)
            {
                FormFactory.Instance<MainForm>().RaiseAhkFunctionResultReportedEvent();
            }
        }

        private void ToggleAutomationEngineVisibility()
        {
            if (VisibleChildForm != null)
            {
                VisibleChildForm.Visible = false;
            }
            else
            {
                FormFactory.Instance<MainForm>().ToggleVisibility();
            }
        }
    }
}