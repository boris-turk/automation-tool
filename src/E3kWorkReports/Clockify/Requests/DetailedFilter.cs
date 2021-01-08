using System.Runtime.Serialization;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class DetailedFilter
    {
        [DataMember(Name = "page")]
        public int Page { get; set; }

        [DataMember(Name = "pageSize")]
        public int PageSize { get; } = 50;

        [DataMember(Name = "sortColumn")]
        public string SortColumn { get; } = "DATE";

        public bool IsOnLastPage(int totalCount)
        {
            return Page * PageSize  >= totalCount;
        }
    }
}