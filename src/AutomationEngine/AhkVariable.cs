using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AhkVariable : AbstractValue
    {
        [XmlAttribute]
        public string Context { get; set; }

        public bool ContextSpecified => !string.IsNullOrWhiteSpace(Context);

        private ContextVariableMappingsCollection Mappings => ContextVariableMappingsCollection.Instance;

        public override string InteropValue
        {
            get
            {
                string replacement = Mappings.GetMappedValue(Value);
                if (replacement != null)
                {
                    return replacement;
                }
                return Value;
            }
        }
    }
}