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
            Arguments = new List<AbstractValue>();
        }

        public string Name { get; set; }

        public string Context { get; set; }

        public bool ContextSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Context); }
        }

        [XmlElement("Argument", typeof(StringValue))]
        [XmlElement("AhkArgument", typeof(AhkVariable))]
        public List<AbstractValue> Arguments { get; set; }

        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }
    }
}
