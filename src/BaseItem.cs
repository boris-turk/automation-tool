using System;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class BaseItem
    {
        public BaseItem()
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

        public string Pattern { get; set; }

        public bool PatternSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Pattern); }
        }
    }
}