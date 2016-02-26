using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutomationEngine.Messages;

namespace AutomationEngine
{
    public partial class MainForm : Form
    {
        private const int OutOfScreenOffset = -20000;
        public static event Action AhkFunctionResultReported;

        public MainForm()
        {
            Text = "Automation engine";
            InitializeComponent();
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(OutOfScreenOffset, OutOfScreenOffset);
            Closing += (sender, args) =>
            {
                args.Cancel = true;
                Visible = false;
            };
        }

        public Label StackLabel
        {
            get { return _stackLabel; }
        }

        public TextBox TextBox
        {
            get { return _textBox; }
        }

        public ListBox ListBox
        {
            get { return _listBox; }
        }

        public bool WorkInProgressVisible
        {
            get { return _workInProgressPictureBox.Visible; }
            set
            {
                _workInProgressPictureBox.Visible = value;
                if (value)
                {
                    _workInProgressPictureBox.BringToFront();
                }
                else
                {
                    
                    _workInProgressPictureBox.SendToBack();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReloadGuard.Start(this);
            LoadMenuEngine();
        }

        public void LoadMenuEngine()
        {
            Menu rootMenu = BuildMenuStructure();
            MenuEngine.Start(this, rootMenu);
        }

        private Menu BuildMenuStructure()
        {
            var menuStorage = new MenuStorage(Configuration.MenusFileName);
            var rootMenu = new Menu
            {
                Id = "root"
            };
            List<Menu> allMenus = menuStorage.LoadMenus().ToList();
            rootMenu.Submenus.AddRange(allMenus);

            foreach (Menu menu in allMenus)
            {
                List<Menu> subMenus = menu.SubmenuIdentifiers
                    .Select(x => allMenus.FirstOrDefault(y => y.Id == x))
                    .Where(x => x != null)
                    .ToList();

                menu.Submenus.AddRange(subMenus);
            }

            return rootMenu;
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

        private void OnWmCopyData(Message message)
        {
            var mystr = new CopyDataStruct();
            Type mytype = mystr.GetType();
            mystr = (CopyDataStruct)message.GetLParam(mytype);

            if (mystr.LpData == WindowMessages.ToggleGLobalMenuVisibility)
            {
                MenuEngine.Instance.Context = null;
                ToggleGlobalAutomationEngineVisibility();
            }
            else if (mystr.LpData == WindowMessages.ToggleContextMenuVisibility)
            {
                MenuEngine.Instance.Context = AhkInterop.GetMessageFileContents().FirstOrDefault();
                ToggleGlobalAutomationEngineVisibility();
            }
            else if (mystr.LpData == WindowMessages.AhkFunctionResultReported)
            {
                if (AhkFunctionResultReported != null)
                {
                    AhkFunctionResultReported();
                }
            }
        }

        private void ToggleGlobalAutomationEngineVisibility()
        {
            Visible = !Visible;
            if (Visible)
            {
                if (Location.X == OutOfScreenOffset)
                {
                    CenterToScreen();
                }
                MenuEngine.Instance.ClearSearchBar();
                TopMost = true;
                Activate();
            }
        }
    }
}
