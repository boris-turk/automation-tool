using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    [XmlRoot("ExecutableItems")]
    public class ExecutableItemsCollection
    {
        private string _fileName;

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

        public void SaveToFile()
        {
            if (_fileName == null)
            {
                throw new InvalidOperationException("File name not specified.");
            }
            XmlStorage.Save(_fileName, this);
        }

        public static ExecutableItemsCollection LoadFromFile(string fileName)
        {
            var executableItems = XmlStorage.Load<ExecutableItemsCollection>(fileName);
            if (executableItems == null)
            {
                executableItems = new ExecutableItemsCollection();
            }
            executableItems._fileName = fileName;
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
