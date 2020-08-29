﻿using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public class FilterAlgorithm
    {
        private readonly string _filterText;

        public FilterAlgorithm(string filterText)
        {
            _filterText = filterText;
        }

        public IEnumerable<SearchItem> Filter(IEnumerable<SearchItem> items)
        {
            foreach (var item in items)
            {
                if (MatchesFilter(item.Text))
                    yield return item;
            }
        }

        private bool MatchesFilter(string text)
        {
            return text.StartsWith(_filterText);
        }
    }
}