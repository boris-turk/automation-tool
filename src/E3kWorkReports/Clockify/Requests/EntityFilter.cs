using System.Collections.Generic;
using System.Runtime.Serialization;

// ReSharper disable UnusedMember.Global

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class EntityFilter
    {
        public string ContainsFlag = "CONTAINS";
        public string DoesNotContainFlag = "DOES_NOT_CONTAIN";

        public EntityFilter()
        {
        }

        public EntityFilter(params string[] ids)
        {
            Ids = new List<string>(ids);
            Contains = ContainsFlag;
        }

        [DataMember(Name = "ids")]
        public List<string> Ids { get; set; }

        [DataMember(Name = "contains")]
        public string Contains { get; }

        [DataMember(Name = "status")]
        public string Status { get; } = "ALL";
    }
}