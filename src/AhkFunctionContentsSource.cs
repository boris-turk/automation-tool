using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AhkFunctionContentsSource
    {
        public AhkFunctionContentsSource()
        {
            Arguments = new List<object>();
        }

        public string Function { get; set; }

        [XmlElement("Argument", typeof(StringValue))]
        [XmlElement("AhkArgument", typeof(AhkVariable))]
        public List<object> Arguments { get; set; }
    }
}