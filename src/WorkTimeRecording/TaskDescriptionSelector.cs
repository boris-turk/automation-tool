using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutomationEngine;

namespace WorkTimeRecording
{
    public class TaskDescriptionSelector : IPluginLoader
    {
        private string GetProject()
        {
            string name = MenuEngine.Instance.GetExecutingItemName();
            string text = (name ?? string.Empty).Trim();
            return Regex.Replace(text, @"\s.*", "");
        }

        private string GetTask()
        {
            string name = MenuEngine.Instance.GetExecutingItemName();
            string text = (name ?? string.Empty).Trim();
            return Regex.Replace(text, @"^\S+\s+", "");
        }

        public string Id => "select_working_time_task";

        public List<BaseItem> Load()
        {
            string project = GetProject();
            string task = GetTask();

            List<BaseItem> items = (
                from entry in WorkingTimeStorage.Instance.Entries
                where entry.Project == project && entry.Task == task
                select new ExecutableItem
                {
                    Name = entry.Description,
                    Arguments = new List<AbstractValue>
                    {
                        new StringValue { Value = entry.Project },
                        new StringValue { Value = entry.Task },
                        new StringValue { Value = entry.Description }
                    }
                })
                .Cast<BaseItem>()
                .ToList();

            BaseItem newEntryItem = new ExecutableItem
            {
                Name = "Nov vnos",
                Arguments = new List<AbstractValue>
                {
                    new StringValue { Value = project },
                    new StringValue { Value = task }
                }
            };

            items.Insert(0, newEntryItem);

            return items;
        }
    }
}