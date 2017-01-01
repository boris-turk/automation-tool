using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class PluginContentSource
    {
        [XmlText]
        public string SourceId { get; set; }
    }
}