using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Settings
    {
        [DataMember(Name = "collapseAllProjectLists")]
        public bool CollapseAllProjectLists { get; set; }

        [DataMember(Name = "dashboardPinToTop")]
        public bool DashboardPinToTop { get; set; }

        [DataMember(Name = "dashboardSelection")]
        public string DashboardSelection { get; set; }

        [DataMember(Name = "dashboardViewType")]
        public string DashboardViewType { get; set; }

        [DataMember(Name = "dateFormat")]
        public string DateFormat { get; set; }

        [DataMember(Name = "isCompactViewOn")]
        public bool IsCompactViewOn { get; set; }

        [DataMember(Name = "longRunning")]
        public bool LongRunning { get; set; }

        [DataMember(Name = "projectListCollapse")]
        public object ProjectListCollapse { get; set; }

        [DataMember(Name = "sendNewsletter")]
        public bool SendNewsletter { get; set; }

        [DataMember(Name = "summaryReportSettings")]
        public SummaryReportSettings SummaryReportSettings { get; set; }

        [DataMember(Name = "timeFormat")]
        public string TimeFormat { get; set; }

        [DataMember(Name = "timeTrackingManual")]
        public bool TimeTrackingManual { get; set; }

        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "weekStart")]
        public string WeekStart { get; set; }

        [DataMember(Name = "weeklyUpdates")]
        public bool WeeklyUpdates { get; set; }
    }
}
