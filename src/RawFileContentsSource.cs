using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class RawFileContentsSource : AhkContentSource
    {
        public string Path { get; set; }

        public override string ReturnType => "ResultFromRawFile";

        public override string Function => string.Empty;

        [XmlIgnore]
        public override List<AbstractValue> InteropArguments
        {
            get
            {
                List<AbstractValue> arguments = base.InteropArguments;
                arguments.Insert(0, new StringValue { Value = Path });
                return arguments;
            }
        }
    }
}