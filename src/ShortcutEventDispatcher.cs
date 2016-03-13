using System;
using System.IO;
using System.Windows.Forms;

namespace AutomationEngine
{
    public class ShortcutEventDispatcher
    {
        private readonly KeyEventArgs _keyEventArgs;
        private readonly MenuEntryDeletion _menuEntryDeletion;

        public ShortcutEventDispatcher(MenuState state, KeyEventArgs keyEventArgs)
        {
            _menuEntryDeletion = new MenuEntryDeletion(state);
            _keyEventArgs = keyEventArgs;
        }

        public bool IsDelete
        {
            get { return _keyEventArgs.KeyCode == Keys.Delete; }
        }

        public void Dispatch()
        {
            if (IsDelete)
            {
                _menuEntryDeletion.Delete();
            }
        }
    }
}