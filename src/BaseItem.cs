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
        public List<Word> NameWords { get; set; }

        public bool NameWordsSpecified
        {
            get { return NameWords != null && NameWords.Any(); }
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
            return string.Join(" ", NameWords.Select(x => x.DisplayValue));
        }

        private void InitializePattern()
        {
            if (string.IsNullOrEmpty(Name))
            {
                NameWords = null;
                return;
            }

            NameWords = new List<Word>();
            foreach (string word in Name.Split(' '))
            {
                AddWord(word);
            }
        }

        private void AddWord(string word)
        {
            if (string.Equals(word, Context, StringComparison.InvariantCultureIgnoreCase))
            {
                AddContextWord();
            }
            else
            {
                AddNormalWord(word);
            }
        }

        private void AddNormalWord(string word)
        {
            NameWords.Add(new Word
            {
                Value = word
            });
        }

        private void AddContextWord()
        {
            NameWords.Add(new Word
            {
                Value = Context,
                IsContext = true
            });
        }
    }
}