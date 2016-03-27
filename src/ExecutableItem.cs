using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class ExecutableItem : BaseItem
    {
        public ExecutableItem()
        {
            Arguments = new List<AbstractValue>();
        }

        [XmlElement("Argument", typeof(StringValue))]
        [XmlElement("AhkArgument", typeof(AhkVariable))]
        public List<AbstractValue> Arguments { get; set; }

        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }

        public ExecutableItem Clone()
        {
            return Cloner.Clone(this);
        }
    }
}
