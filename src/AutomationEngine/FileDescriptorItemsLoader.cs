using System.Collections.Generic;

namespace AutomationEngine
{
    public class FileDescriptorItemsLoader : IItemsLoader
    {
        private readonly string _path;

        public FileDescriptorItemsLoader(string path)
        {
            _path = path;
        }

        public List<BaseItem> Load()
        {
            return Menu.LoadFromFile<Menu>(_path).Items;
        }
    }
}