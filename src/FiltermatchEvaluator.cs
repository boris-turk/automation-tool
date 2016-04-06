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

        public List<PatternPart> NamePatterns
        {
            get { return _item.NamePatterns; }
        }

        public bool CheckForPerfectMatch { get; set; }

        public bool MatchesFilter { get; private set; }

        public void Evaluate()
        {
            MatchesFilter = false;
            _item.IsPerfectMatch = false;

            if (!_filterWords.Any())
            {
                return;
            }

            if (!NamePatterns.Any())
            {
                MatchesFilter = true;
                return;
            }

            if (CheckForPerfectMatch)
            {
                TestForPerfectMatch();
            }
            else
            {
                TestForNormalMatch();
            }
        }

        private void TestForPerfectMatch()
        {
            string[] filterWords;

            if (NamePatterns.First().IsMatch(_filterWords.First()))
            {
                _item.IsPerfectMatch = true;
                filterWords = _filterWords.Skip(1).ToArray();
            }
            else
            {
                _item.IsPerfectMatch = false;
                filterWords = _filterWords.ToArray();
            }

            MatchesFilter = filterWords.All(x => NamePatterns.Any(y => y.IsMatch(x)));
        }

        private void TestForNormalMatch()
        {
            MatchesFilter = _filterWords.All(x => NamePatterns.Any(y => y.IsMatch(x)));
        }
    }
}