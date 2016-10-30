using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AutomationArgument : AbstractValue
    {
        public override string InteropValue
        {
            get
            {
                if (Value == "ActiveItemId")
                {
                    return "\"" + "ActiveItemId" + "\"";
                }
                if (Value == "ActiveMenuFilePath")
                {
                    return "\"" + "ActiveMenuFilePath" + "\"";
                }
                return Value;
            }
        }
    }
}