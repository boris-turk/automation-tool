using System.Xml.Serialization;

namespace AutomationEngine
{
    public class AutomationAction
    {
        public AutomationAction()
        {
            Shortcut = new Shortcut();
        }

        [XmlElement("Type")]
        public ActionType ActionType { get; set; }

        public Shortcut Shortcut { get; set; }
    }
}