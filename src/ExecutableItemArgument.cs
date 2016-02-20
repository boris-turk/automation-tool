using System;

namespace AutomationEngine
{
    [Serializable]
    public class ExecutableItemArgument
    {
        public ArgumentType Type { get; set; }
        public string Value { get; set; }
    }
}
