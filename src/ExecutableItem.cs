using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class ExecutableItem
    {
        public ExecutableItem()
        {
            Arguments = new List<ExecutableItemArgument>();
        }

        public string Name { get; set; }

        [XmlElement("Argument")]
        public List<ExecutableItemArgument> Arguments { get; set; }

        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }
    }
}
