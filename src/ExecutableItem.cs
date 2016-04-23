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

        [XmlIgnore]
        public bool ExecutingMethodNameAssignedAtRuntime { get; set; }

        [XmlElement("Argument", typeof(StringValue))]
        [XmlElement("AhkArgument", typeof(AhkVariable))]
        public List<AbstractValue> Arguments { get; set; }

        public ExecutableItem Clone()
        {
            return Cloner.Clone(this);
        }

        public string ExecutingMethodName { get; set; }

        [XmlIgnore]
        public string ReplacedItemId { get; set; }
    }
}
