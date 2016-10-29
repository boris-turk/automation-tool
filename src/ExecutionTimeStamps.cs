using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class ExecutionTimeStamps : FileStorage<ExecutionTimeStamps>
    {
        public override string StorageFileName => @"execution_timestamps.xml";

        public ExecutionTimeStamps()
        {
            Entries = new List<TimeStamp>();
        }

        public List<TimeStamp> Entries { get; set; }

        public void SetTimeStamp(string id, DateTime now)
        {
            TimeStamp entry = Entries.SingleOrDefault(x => x.ItemId == id);

            if (entry == null)
            {
                entry = new TimeStamp
                {
                    ItemId = id
                };
                Entries.Add(entry);
            }

            entry.DateTime = now;
        }

        public DateTime GetTimeStamp(string id)
        {
            TimeStamp entry = Entries.SingleOrDefault(x => x.ItemId == id);
            if (entry != null)
            {
                return entry.DateTime;
            }
            return DateTime.MinValue;
        }
    }
}