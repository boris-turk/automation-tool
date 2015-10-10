using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ahk.Messages;

namespace Ahk
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Text = "Automation engine";
            InitializeComponent();
            TopMost = true;
            StartPosition = FormStartPosition.CenterScreen;
        }

        public Label StackLabel => _stackLabel;

        public TextBox TextBox => _textBox;

        public ListBox ListBox => _listBox;

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
			
			base.WndProc(ref m);
		}

        private void OnWmCopyData(Message message)
        {
            var mystr = new CopyDataStruct();
            Type mytype = mystr.GetType();
            mystr = (CopyDataStruct)message.GetLParam(mytype);

            if (mystr.LpData == WindowMessages.ToggleVisibility)
            {
                ToggleVisibility();
            }
        }

        private void ToggleVisibility()
        {
            Visible = !Visible;
            if (Visible)
            {
                TopMost = true;
                Activate();
            }
        }
    }
}
