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

        [XmlIgnore]
        public string ReplacedItemId { get; set; }

        public ActionType ActionType { get; set; }

        public bool ActionTypeSpecified
        {
            get { return ActionType != ActionType.None; }
        }
    }
}
