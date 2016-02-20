using System;

namespace AutomationEngine
{
    [Serializable]
    public class AhkVariable : AbstractValue
    {
        public override string InteropValue
        {
            get { return Value; }
        }
    }
}