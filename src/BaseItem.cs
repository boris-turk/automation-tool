using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class BaseItem
    {
        private string _name;

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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                InitializePattern();
            }
        }

        public RegularExpression ArgumentsRegex { get; set; }

        public bool ArgumentsRegexSpecified
        {
            get { return ArgumentsRegex != null; }
        }

        public bool NameSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }

        [XmlIgnore]
        public List<PatternPart> NamePatterns { get; set; }

        public bool NamePatternsSpecified
        {
            get { return NamePatterns != null && NamePatterns.Any(); }
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
            return string.Join(" ", NamePatterns.Select(x => x.DisplayValue));
        }

        private void InitializePattern()
        {
            if (string.IsNullOrEmpty(Name))
            {
                NamePatterns = null;
                return;
            }

            NamePatterns = new List<PatternPart>();
            foreach (string word in Name.Split(' '))
            {
                if (string.Equals(word, Context, StringComparison.InvariantCultureIgnoreCase))
                {
                    NamePatterns.Add(new Word
                    {
                        Value = Context,
                        IsContext = true
                    });
                }
                else
                {
                    NamePatterns.Add(new Word
                    {
                        Value = word
                    });
                }
            }
        }
    }
}