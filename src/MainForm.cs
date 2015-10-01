﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ahk.Messages;
using AutoHotkey.Interop;

namespace Ahk
{
    public partial class MainForm : Form
    {
        private readonly AutoHotkeyEngine _ahk;

        public MainForm()
        {
            InitializeComponent();
            TopMost = true;
            StartPosition = FormStartPosition.CenterScreen;
            _ahk = new AutoHotkeyEngine();
        }

        public Label StackLabel => _stackLabel;

        public TextBox TextBox => _textBox;

        public ListBox ListBox => _listBox;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeMenuEngine();
            InstallMainShortcut();
        }

        private void InitializeMenuEngine()
        {
            var menuStorage = new MenuStorage(Configuration.MenusFileName);
            var rootMenu = new Menu
            {
                Id = "root"
            };
            List<Menu> menus = menuStorage.LoadMenus().ToList();
            rootMenu.Submenus.AddRange(menus);
            new MenuEngine(this, rootMenu, _ahk);
        }

        private void InstallMainShortcut()
        {
            string handle = Handle.ToInt32().ToString("X");

            var definition = new StringBuilder();
            definition.AppendLine("!9::");
            definition.AppendLine("StringToSend = " + WindowMessages.ToggleVisibility);
            definition.AppendLine("VarSetCapacity(CopyDataStruct, 3*A_PtrSize, 0)");
            definition.AppendLine("SizeInBytes := (StrLen(StringToSend) + 1) * (A_IsUnicode ? 2 : 1)");
            definition.AppendLine("NumPut(SizeInBytes, CopyDataStruct, A_PtrSize)");
            definition.AppendLine("NumPut(&StringToSend, CopyDataStruct, 2*A_PtrSize)");
            definition.AppendLine("Prev_DetectHiddenWindows := A_DetectHiddenWindows");
            definition.AppendLine("Prev_TitleMatchMode := A_TitleMatchMode");
            definition.AppendLine("DetectHiddenWindows On");
            definition.AppendLine("SetTitleMatchMode 2");
            definition.AppendLine("SendMessage, 0x4a, 0, &CopyDataStruct,, ahk_id 0x" + handle);
            definition.AppendLine("DetectHiddenWindows %Prev_DetectHiddenWindows%");
            definition.AppendLine("SetTitleMatchMode %Prev_TitleMatchMode%");
            definition.AppendLine("return");

            _ahk.ExecRaw(definition.ToString());
            _ahk.Load(Path.Combine(Configuration.RootDirectory, Configuration.AhkScriptFileName));
            _ahk.ExecFunction("DefineVariables");
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
