using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine
{
    public class FilterAlgorithm
    {
        private readonly string[] _words;

        public FilterAlgorithm(string filterText)
        {
            _words = Regex.Split(filterText ?? "", @"\s")
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .ToArray();
        }

        public IEnumerable<Request> Filter(IEnumerable<Request> items)
        {
            foreach (var item in items)
            {
                if (MatchesFilter(item.Text))
                    yield return item;
            }
        }

        private bool MatchesFilter(string text)
        {
            return !_words.Any() || _words.All(_ => ContainsWord(text, _));
        }

        private bool ContainsWord(string text, string word)
        {
            return text.IndexOf(word, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}