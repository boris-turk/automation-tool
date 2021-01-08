using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class CustomField
    {
        [DataMember(Name = "customFieldId")]
        public string CustomFieldId { get; set; }

        [DataMember(Name = "value")]
        public long Value { get; set; }
    }
}