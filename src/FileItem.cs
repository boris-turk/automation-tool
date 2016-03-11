using System;
using System.IO;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class FileItem : ExecutableItem
    {
        [XmlIgnore]
        public string Directory { get; set; }

        public string FilePath
        {
            get
            {
                if (Directory != null)
                {
                    return Path.Combine(Directory, Arguments[0].Value);
                }
                if (Arguments.Count == 0)
                {
                    throw new InvalidOperationException("FileItem: file path not specified");
                }
                return Arguments[0].Value;
            }
        }
    }
}