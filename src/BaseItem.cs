﻿using System;
using System.Linq;

namespace AutomationEngine
{
    [Serializable]
    public class BaseItem
    {
        public BaseItem()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public virtual string Name { get; set; }

        public string Context { get; set; }

        public bool ContextSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Context); }
        }

        public string ContextGroupId { get; set; }

        public bool ContextGroupIdSpecified
        {
            get
            {
                if (ContextSpecified)
                {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(ContextGroupId);
            }
        }

        public ContextGroup ContextGroup
        {
            get
            {
                return ContextGroupCollection.Instance.Groups.First(x => x.Id == ContextGroupId);
            }
        }
    }
}