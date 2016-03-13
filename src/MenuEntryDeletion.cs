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

            if (_state.ExecutableItemsCollection.Items.Count == 0)
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
            ExecutableItem executableItem = _state.GetExecutableItem();
            if (executableItem == null)
            {
                return;
            }

            _state.ExecutableItemsCollection.Items.Remove(executableItem);
            _state.ExecutableItemsCollection.SaveToFile();

            var fileItem = executableItem as FileItem;
            if (fileItem != null)
            {
                File.Delete(fileItem.FilePath);
            }
        }

        private void DeleteMenu()
        {
            Menu selectedMenu = _state.MatchingSubmenus[_state.SelectedIndex];
            _state.RootMenu.RemoveSubmenu(selectedMenu);
            new MenuStorage().SaveMenuStructure(_state.RootMenu);

            var contentSource = selectedMenu.ContentSource as FileDescriptorContentSource;
            if (contentSource != null)
            {
                File.Delete(contentSource.Path);
            }
        }
    }
}