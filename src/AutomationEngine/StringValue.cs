using System;

namespace AutomationEngine
{
    [Serializable]
    public class StringValue : AbstractValue
    {
        public override string InteropValue => "\"" + Value + "\"";

        public override bool IsEmpty => Value == null;
    }
}
