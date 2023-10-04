using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BTurk.Automation.Core.Serialization
{
    public class ContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (!property.HasMemberAttribute || !property.Writable)
            {
                property.ShouldSerialize = _ => false;
                property.ShouldDeserialize = _ => false;
            }

            return property;
        }
    }
}