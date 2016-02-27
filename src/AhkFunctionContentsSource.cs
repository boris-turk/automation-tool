using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AhkFunctionContentsSource : AhkContentSource
    {
        public AhkFunctionContentsSource()
        {
            Arguments = new List<AbstractValue>();
        }

        [XmlElement("Argument", typeof(StringValue))]
        [XmlElement("AhkArgument", typeof(AhkVariable))]
        public List<AbstractValue> Arguments { get; set; }

        public override string ReturnType
        {
            get { return "ResultFromFunction"; }
        }
    }
}