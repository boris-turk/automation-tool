using System.Xml.Serialization;

namespace AutomationEngine
{
    public class NamePart
    {
        [XmlAttribute]
        public NamePartType Type { get; set; }

        [XmlText]
        public string Value { get; set; }

        public bool ValueSpecified => Type == NamePartType.Constant;

        [XmlAttribute]
        public int MenuIndex { get; set; }

        public bool MenuIndexSpecified => Type == NamePartType.ReferencedMenu && MenuIndex > 0;
    }
}