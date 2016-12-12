using System;
using System.Xml.Serialization;
using AutomationEngine;

namespace WorkTimeRecording
{
    public class WorkingTimeEntry
    {
        public string Project { get; set; }

        public string Task { get; set; }

        public string Description { get; set; }

        [XmlIgnore]
        public TimeSpan Duration { get; set; }

        public DateTime Date { get; set; }

        [XmlElement("Duration")]
        public string DurationString
        {
            get { return Duration.ToTimeSpanString(); }
            set { Duration = value.FromTimeSpanString(); }
        }

        public bool ExcludedFromWorkingTime { get; set; }
    }
}