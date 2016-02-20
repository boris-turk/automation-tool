using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class ExecutableItem
    {
        public ExecutableItem()
        {
            Arguments = new List<object>();
        }

        public string Name { get; set; }

        [XmlElement("Argument", typeof(StringValue))]
        [XmlElement("AhkArgument", typeof(AhkVariable))]
        public List<object> Arguments { get; set; }

        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }
    }
}
