using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class Configuration : FileStorage<Configuration>
    {
        public static string MenusFileName = "menus.xml";
        public static string ContextPlaceholder = "$context$";

        public Configuration()
        {
            Actions = new List<AutomationAction>();
        }

        [XmlArrayItem("Action")]
        public List<AutomationAction> Actions { get; set; }

        public override string StorageFileName
        {
            get { return "automation_configuration.xml"; }
        }

        public int ArchiveDayCountThreshold { get; set; }

        public string CurrentContext { get; set; }
    }
}
