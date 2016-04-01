using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AhkVariable : AbstractValue
    {
        [XmlAttribute]
        public string Context { get; set; }

        public bool ContextSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Context); }
        }

        private ContextVariableMappingsCollection Mappings
        {
            get { return ContextVariableMappingsCollection.Instance; }
        }

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