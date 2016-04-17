using System;
using System.Collections.Generic;

namespace AutomationEngine
{
    public class MenuComparer : IComparer<BaseItem>
    {
        private readonly MenuState _state;

        public MenuComparer(MenuState state)
        {
            _state = state;
        }

        public int Compare(BaseItem x, BaseItem y)
        {
            if (x.Context != null && y.Context != null)
            {
                string activeContext = ContextCollection.Instance.Current;

                if (x.Context == activeContext && x.Context != y.Context)
                {
                    return -1;
                }
                if (y.Context == activeContext && x.Context != y.Context)
                {
                    return 1;
                }
            }
            if (x.MatchScore != y.MatchScore)
            {
                return y.MatchScore.CompareTo(x.MatchScore);
            }
            if (_state.Filter.Length == 0)
            {
                return CompareByLastAccessTime(x, y);
            }
            if (FilterMatchesAtStart(x) && !FilterMatchesAtStart(y))
            {
                return -1;
            }
            if (!FilterMatchesAtStart(x) && FilterMatchesAtStart(y))
            {
                return 1;
            }
            return CompareByLastAccessTime(x, y);
        }

        private int CompareByLastAccessTime(BaseItem x, BaseItem y)
        {
            return y.LastAccess.CompareTo(x.LastAccess);
        }

        private bool FilterMatchesAtStart(BaseItem item)
        {
            int index = item.GetProperName().IndexOf(_state.Filter, StringComparison.OrdinalIgnoreCase);
            return index == 0;
        }
    }
}