using System.IO;
using System.Windows.Forms;

namespace AutomationEngine
{
    public class MenuEntryDeletion
    {
        private readonly MenuState _state;

        public MenuEntryDeletion(MenuState state)
        {
            _state = state;
        }

        public void Delete()
        {
            MessageWindow window = new MessageWindow
            {
                Caption = "Delete entry",
                Message = "Delete the selected menu entry?",
                Type = MessageBoxButtons.YesNo

            };
            window.Show();

            if (window.Result != DialogResult.Yes)
            {
                return;
            }

            if (_state.IsMenuSelected)
            {
                DeleteMenu();
            }
            else
            {
                DeleteExecutableItem();
            }
        }

        private void DeleteExecutableItem()
        {
            ExecutableItem executableItem = _state.SelectedExecutableItem;
            if (executableItem == null)
            {
                return;
            }

            _state.SelectedMenu.Items.Remove(executableItem);
            _state.SelectedMenu.SaveToFile();

            var fileItem = executableItem as FileItem;
            if (fileItem != null)
            {
                File.Delete(fileItem.FilePath);
            }
        }

        private void DeleteMenu()
        {
            Menu selectedMenu = _state.SelectedMenu;
            _state.RootMenu.RemoveItem(selectedMenu);
            _state.RootMenu.SaveToFile();

            var contentSource = selectedMenu.ContentSource as FileDescriptorContentSource;
            if (contentSource != null)
            {
                File.Delete(contentSource.Path);
            }
        }
    }
}