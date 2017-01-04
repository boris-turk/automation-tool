using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationEngine;

namespace WorkTimeRecording
{
    public class WorkingTimeStorage
    {
        private static readonly WorkingTimeStorage TheInstance = LoadFromFile();

        public WorkingTimeStorage()
        {
            Entries = new List<WorkingTimeEntry>();
        }

        public static WorkingTimeStorage Instance => TheInstance;

        private static string CurrentMonthFile => GetFilePath(DateTime.Now.Date);

        private static string PreviousMonthFile => GetFilePath(DateTime.Now.Date.AddMonths(-1));

        public List<WorkingTimeEntry> Entries { get; set; }

        private static string GetFilePath(DateTime date)
        {
            return $@"xlab\working_time_{date.Year}_{date.Month:00}.xml";
        }

        private static WorkingTimeStorage LoadFromFile()
        {
            WorkingTimeStorage storage;
            if (File.Exists(CurrentMonthFile))
            {
                storage = XmlStorage.Load<WorkingTimeStorage>(CurrentMonthFile);
            }
            else
            {
                storage = new WorkingTimeStorage();
            }

            if (!File.Exists(PreviousMonthFile))
            {
                return storage;
            }

            WorkingTimeStorage previousMonthStorage = XmlStorage.Load<WorkingTimeStorage>(PreviousMonthFile);

            storage.Entries.AddRange(previousMonthStorage.Entries);

            return storage;
        }

        public void Save()
        {
            List<WorkingTimeEntry> entriesCopy = Entries.ToList();
            Entries.RemoveAll(x => x.Date.Month != DateTime.Now.Month);
            XmlStorage.Save(CurrentMonthFile, this);
            Entries = entriesCopy;
        }
    }
}