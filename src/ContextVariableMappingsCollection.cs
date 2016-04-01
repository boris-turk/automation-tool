using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("ContextVariableMappings")]
    public class ContextVariableMappingsCollection : FileStorage<ContextVariableMappingsCollection>
    {
        public override string StorageFileName
        {
            get { return "context_variable_mapping.xml"; }
        }

        public ContextVariableMappingsCollection()
        {
            Mappings = new List<ContextVariableMapping>();
        }

        [XmlElement("Mapping")]
        public List<ContextVariableMapping> Mappings { get; set; }

        public string GetMappedValue(string originalValue)
        {
            string context = Contexts.Instance.Current;

            ContextVariableMapping mapping = Mappings.FirstOrDefault(x =>
                x.SourceAhkVariable.Value == originalValue &&
                x.TargetAhkVariable.Context == context);

            if (mapping != null)
            {
                return mapping.TargetAhkVariable.Value;
            }

            return null;
        }
    }
}