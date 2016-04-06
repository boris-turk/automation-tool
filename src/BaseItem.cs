using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class BaseItem
    {
        protected BaseItem()
        {
            Id = Guid.NewGuid().ToString();
        }

        [XmlIgnore]
        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified
        {
            get { return LastAccess != DateTime.MinValue; }
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool NameSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }

        public PatternCollection Pattern { get; set; }

        public bool PatternSpecified
        {
            get { return Pattern != null && Pattern.LeadingParts.Any(); }
        }

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

        [XmlIgnore]
        public bool IsPerfectMatch { get; set; }

        public string GetProperName()
        {
            if (NameSpecified)
            {
                return Name;
            }

            return string.Join(" ", Pattern.LeadingParts.Select(x => x.DisplayValue));
        }
    }
}