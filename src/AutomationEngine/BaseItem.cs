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

        [XmlIgnore]
        public DateTime LastAccess { get; set; }

        public bool LastAccessSpecified => LastAccess != DateTime.MinValue;

        public string Id { get; set; }

        public string Alias { get; set; }

        public bool AliasSpecified => !string.IsNullOrWhiteSpace(Alias);

        public string ExecutingMethodName { get; set; }

        public bool ExecutingMethodNameSpecified => !string.IsNullOrWhiteSpace(ExecutingMethodName);

        public VisibilityCondition VisibilityCondition { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                InitializePattern();
            }
        }

        public ActionType ActionType { get; set; }

        public bool ActionTypeSpecified => ActionType != ActionType.None;

        public bool NameSpecified => !string.IsNullOrWhiteSpace(Name);

        [XmlIgnore]
        public Menu ParentMenu { get; set; }

        [XmlIgnore]
        public List<Word> NameWords { get; set; }

        public bool NameWordsSpecified => NameWords != null && NameWords.Any();

        public string Context { get; set; }

        public bool ContextSpecified => !string.IsNullOrWhiteSpace(Context);

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
        public int MatchScore { get; set; }

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
            foreach (string word in Name.Split(' ', '\t'))
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