using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class FilterMatchEvaluator
    {
        private readonly BaseItem _item;
        private readonly string[] _filterWords;

        public FilterMatchEvaluator(BaseItem item, string[] filterWords)
        {
            _item = item;
            _filterWords = filterWords;
        }

        private string CurrentContext => Configuration.Instance.CurrentContext;

        public List<Word> NameWords => _item.NameWords;

        public void Evaluate()
        {
            if (!_filterWords.Any())
            {
                // no filter specified => all items match with same score
                _item.MatchScore = 1;
                return;
            }

            _item.MatchScore = 0;

            if (!NameWords.Any())
            {
                // name not specified => item is not properly defined, filter it out
                _item.MatchScore = 0;
                return;
            }

            List<Word> nameWords = NameWords.ToList();
            for (int i = 0; i < _filterWords.Length; i++)
            {
                if (nameWords.Count == 0)
                {
                    _item.MatchScore = 0;
                    return;
                }

                // get name word with highest match score for the current filter word
                var element = nameWords
                    .Select((value, index) => new
                    {
                        index,
                        matchScore = value.MatchScore(_filterWords[i])
                    })
                    .OrderByDescending(x => x.matchScore)
                    .First();

                int multiplier = 1;
                if (WordEqualsCurrentContext(nameWords[element.index]))
                {
                    multiplier *= 2;
                }
                if (element.index == 0 && i == 0)
                {
                    // first word match is more important, multiply its score by 5
                    multiplier *= 5;
                }

                _item.MatchScore += element.matchScore * multiplier;

                if (element.matchScore == 0)
                {
                    // one of the name words did not match => whole item does not match
                    _item.MatchScore = 0;
                    return;
                }

                // the best matched word is no longer included in match evaluation
                nameWords.RemoveAt(element.index);
            }

            if (_item.MatchScore > 0 && CurrentContext != null && _item.Context == CurrentContext)
            {
                _item.MatchScore += 1;
            }
        }

        private bool WordEqualsCurrentContext(Word word)
        {
            return string.CompareOrdinal(word.DisplayValue, CurrentContext) == 0;
        }
    }
}