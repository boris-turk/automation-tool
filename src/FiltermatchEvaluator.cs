using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class FilterMatchEvaluator
    {
        private FilterMatch _filterMatch;

        private readonly BaseItem _item;
        private readonly string[] _filterWords;

        public FilterMatchEvaluator(BaseItem item, string[] filterWords)
        {
            _item = item;
            _filterWords = filterWords;
        }

        public PatternCollection Pattern
        {
            get { return _item.Pattern; }
        }

        public bool MatchesFilter
        {
            get { return _filterMatch != null; }
        }

        public void Evaluate()
        {
            List<PatternPart> patternParts = Pattern.LeadingParts;

            _filterMatch = null;
            for (int i = 0; i < patternParts.Count; i++)
            {
                if (i >= _filterWords.Length)
                {
                    _filterMatch = new FilterMatch();
                    return;
                }
                PatternPart patternItem = patternParts[i];
                if (!patternItem.IsMatch(_filterWords[i]))
                {
                    return;
                }
            }

            if (_filterWords.Length > patternParts.Count)
            {
                if (!(patternParts.Last() is RegularExpression))
                {
                    return;
                }

                string rest = string.Join(" ", _filterWords.Skip(patternParts.Count));
                if (patternParts.Last().IsMatch(rest))
                {
                    _filterMatch = new FilterMatch();
                }
                return;
            }

            _filterMatch = new FilterMatch();
        }
    }
}