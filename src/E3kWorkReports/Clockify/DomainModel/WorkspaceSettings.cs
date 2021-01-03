using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class WorkspaceSettings
    {
        [DataMember(Name = "adminOnlyPages")]
        public object[] AdminOnlyPages { get; set; }

        [DataMember(Name = "automaticLock")]
        public AutomaticLock AutomaticLock { get; set; }

        [DataMember(Name = "canSeeTimeSheet")]
        public bool CanSeeTimeSheet { get; set; }

        [DataMember(Name = "canSeeTracker")]
        public bool CanSeeTracker { get; set; }

        [DataMember(Name = "defaultBillableProjects")]
        public bool DefaultBillableProjects { get; set; }

        [DataMember(Name = "forceDescription")]
        public bool ForceDescription { get; set; }

        [DataMember(Name = "forceProjects")]
        public bool ForceProjects { get; set; }

        [DataMember(Name = "forceTags")]
        public bool ForceTags { get; set; }

        [DataMember(Name = "forceTasks")]
        public bool ForceTasks { get; set; }

        //[DataMember(Name = "lockTimeEntries")]
        //public DateTimeOffset LockTimeEntries { get; set; }

        [DataMember(Name = "onlyAdminsCreateProject")]
        public bool OnlyAdminsCreateProject { get; set; }

        [DataMember(Name = "onlyAdminsCreateTag")]
        public bool OnlyAdminsCreateTag { get; set; }

        [DataMember(Name = "onlyAdminsCreateTask")]
        public bool OnlyAdminsCreateTask { get; set; }

        [DataMember(Name = "onlyAdminsSeeAllTimeEntries")]
        public bool OnlyAdminsSeeAllTimeEntries { get; set; }

        [DataMember(Name = "onlyAdminsSeeBillableRates")]
        public bool OnlyAdminsSeeBillableRates { get; set; }

        [DataMember(Name = "onlyAdminsSeeDashboard")]
        public bool OnlyAdminsSeeDashboard { get; set; }

        [DataMember(Name = "onlyAdminsSeePublicProjectsEntries")]
        public bool OnlyAdminsSeePublicProjectsEntries { get; set; }

        [DataMember(Name = "projectFavorites")]
        public bool ProjectFavorites { get; set; }

        [DataMember(Name = "projectGroupingLabel")]
        public string ProjectGroupingLabel { get; set; }

        [DataMember(Name = "projectPickerSpecialFilter")]
        public bool ProjectPickerSpecialFilter { get; set; }

        [DataMember(Name = "round")]
        public Round Round { get; set; }

        [DataMember(Name = "timeRoundingInReports")]
        public bool TimeRoundingInReports { get; set; }

        [DataMember(Name = "trackTimeDownToSecond")]
        public bool TrackTimeDownToSecond { get; set; }

        [DataMember(Name = "isProjectPublicByDefault")]
        public bool IsProjectPublicByDefault { get; set; }

        [DataMember(Name = "featureSubscriptionType")]
        public string FeatureSubscriptionType { get; set; }
    }
}