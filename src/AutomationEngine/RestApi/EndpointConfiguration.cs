using System;

namespace AutomationEngine.RestApi
{
    public class EndpointConfiguration
    {
        public EndpointConfiguration(string name, EndPointType type)
        {
            Type = type;
            Name = name;
        }

        public EndPointType Type { get; set; }

        public string Name { get; set; }
    }
}