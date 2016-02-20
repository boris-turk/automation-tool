using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class FileDescriptorContentSource
    {
        [XmlText]
        public string Path { get; set; }
    }
}