using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class FileGroupCollection : FileStorage<FileGroupCollection>
    {
        public FileGroupCollection()
        {
            FileGroups = new List<FileGroup>();
        }

        public override string StorageFileName
        {
            get { return @"file_groups.xml"; }
        }

        [XmlElement("FileGroup")]
        public List<FileGroup> FileGroups { get; set; }

        public FileGroup GetGroupById(string groupId)
        {
            return FileGroups.FirstOrDefault(x => x.Id == groupId);
        }

        public void AddFileGroup(FileGroup fileGroup)
        {
            FileGroups.Add(fileGroup);
        }
    }
}