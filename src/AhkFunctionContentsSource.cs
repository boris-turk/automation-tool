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
            Arguments = new List<ExecutableItemArgument>();
        }

        public string Function { get; set; }

        [XmlElement("Argument")]
        public List<ExecutableItemArgument> Arguments { get; set; }
    }
}