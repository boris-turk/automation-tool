using System.Collections.Generic;
using System.Runtime.Serialization;
using E3kWorkReports.Clockify.Requests;

// ReSharper disable StringLiteralTypo

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class DetailedReport
    {
        [DataMember(Name = "totals")]
        public List<Total> Totals { get; set; }

        [DataMember(Name = "timeentries")]
        public List<TimeEntry> TimeEntries { get; set; }

        public bool IsWholePage(DetailedReportRequest request)
        {
            return TimeEntries.Count == request.DetailedFilter.PageSize;
        }
    }
}