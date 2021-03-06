﻿using System;
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
        [XmlElement("DynamicArgument", typeof(DynamicValue))]
        [XmlElement("ReferencedItemArgument", typeof(ReferencedItemArgument))]
        public List<AbstractValue> Arguments { get; set; }

        public override BaseItem Clone()
        {
            return Cloner.Clone(this);
        }

        [XmlIgnore]
        public string ReplacedItemId { get; set; }
    }
}
