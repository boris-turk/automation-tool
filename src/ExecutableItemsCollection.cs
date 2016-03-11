using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("ExecutableItems")]
    public class ExecutableItemsCollection
    {
        [XmlElement("Item", typeof(ExecutableItem))]
        [XmlElement("FileItem", typeof(FileItem))]
        public List<ExecutableItem> Items { get; set; }

        public string GroupId { get; set; }

        public FileGroup Group
        {
            get { return FileGroupCollection.Instance.GetGroupById(GroupId); }
        }

        public bool GroupSpecified
        {
            get { return !string.IsNullOrWhiteSpace(GroupId); }
        }

        public ExecutableItemsCollection()
        {
            Items = new List<ExecutableItem>();
        }

        public void SaveToFile(string filePath)
        {
            XmlStorage.Save(filePath, this);
        }

        public static ExecutableItemsCollection LoadFromFile(string filePath)
        {
            var executableItems = XmlStorage.Load<ExecutableItemsCollection>(filePath);
            if (executableItems == null)
            {
                return new ExecutableItemsCollection();
            }
            PrependRootDirectoryToFileItems(executableItems);
            return executableItems;
        }

        private static void PrependRootDirectoryToFileItems(ExecutableItemsCollection executableItems)
        {
            if (executableItems.GroupId == null)
            {
                return;
            }
            foreach (FileItem fileItem in executableItems.Items.OfType<FileItem>())
            {
                fileItem.Directory = executableItems.Group.Directory;
            }
        }
    }
}
