using System.Collections.Generic;
using AutomationEngine;

namespace WorkTimeRecording
{
    public class WorkingTimeStorage : FileStorage<WorkingTimeStorage>
    {
        public WorkingTimeStorage()
        {
            Entries = new List<WorkingTimeEntry>();
        }

        public List<WorkingTimeEntry> Entries { get; set; }

        public override string StorageFileName => @"xlab\working_time.xml";
    }
}