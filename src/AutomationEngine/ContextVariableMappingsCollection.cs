﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("ContextVariableMappings")]
    public class ContextVariableMappingsCollection : FileStorage<ContextVariableMappingsCollection>
    {
        public override string StorageFileName => "context_variable_mapping.xml";

        public ContextVariableMappingsCollection()
        {
            Mappings = new List<ContextVariableMapping>();
        }

        [XmlElement("Mapping")]
        public List<ContextVariableMapping> Mappings { get; set; }

        public string GetMappedValue(string originalValue)
        {
            string context = Configuration.Instance.CurrentContext;

            ContextVariableMapping mapping = Mappings.FirstOrDefault(x =>
                x.SourceAhkVariable.Value == originalValue &&
                x.TargetAhkVariable.Context == context);

            return mapping?.TargetAhkVariable.Value;
        }
    }
}