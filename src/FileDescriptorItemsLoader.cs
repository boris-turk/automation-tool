using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class FileDescriptorItemsLoader : IExecutableItemsLoader
    {
        private readonly string _path;

        public FileDescriptorItemsLoader(string path)
        {
            _path = path;
        }

        public List<ExecutableItem> Load()
        {
            return new MenuStorage(_path).LoadExecutableItems().ToList();
        }
    }
}