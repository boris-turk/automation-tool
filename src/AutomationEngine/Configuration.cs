using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class Configuration : FileStorage<Configuration>
    {
        public static string ContextPlaceholder = "$context$";

        public Configuration()
        {
            Actions = new List<AutomationAction>();
        }

        [XmlArrayItem("Action")]
        public List<AutomationAction> Actions { get; set; }

        public override string StorageFileName => "automation_configuration.xml";

        public int ArchiveDayCountThreshold { get; set; }

        public string CurrentContext { get; set; }

        public string RootMenuAlias { get; set; }

        [XmlArrayItem("Path")]
        public List<string> MenuPaths { get; set; }

        [XmlArrayItem("Path")]
        public List<string> ApplicationMenuPaths { get; set; }
    }
}
