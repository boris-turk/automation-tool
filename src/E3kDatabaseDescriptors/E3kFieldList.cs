using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationEngine;

namespace E3kDatabaseDescriptors
{
    // ReSharper disable once InconsistentNaming
    public class E3kFieldList : IPluginLoader
    {
        private List<BaseItem> _items;
        private DateTime? _lastWriteTime;

        public string Id => "e3k_fields";

        private string FilePath => @"custom\e3k_fields.txt";

        public List<BaseItem> Load()
        {
            if (_lastWriteTime.HasValue)
            {
                DateTime lastWriteTime = File.GetLastWriteTime(FilePath);
                if (lastWriteTime <= _lastWriteTime)
                {
                    return _items;
                }
                _lastWriteTime = lastWriteTime;
            }
            else
            {
                _lastWriteTime = File.GetLastWriteTime(FilePath);
            }

            _items = (
                from line in File.ReadAllLines(FilePath)
                select new ExecutableItem
                {
                    Name = line
                })
                .Cast<BaseItem>()
                .ToList();

            return _items;
        }
    }
}