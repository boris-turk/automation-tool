using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AhkFunctionContentsSource : ContentSource
    {
        public AhkFunctionContentsSource()
        {
            Arguments = new List<AbstractValue>();
        }

        public string Function { get; set; }

        [XmlElement("Argument", typeof (StringValue))]
        [XmlElement("AhkArgument", typeof (AhkVariable))]
        public List<AbstractValue> Arguments { get; set; }
    }
}