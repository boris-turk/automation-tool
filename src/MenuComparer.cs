using System;
using System.Collections.Generic;

namespace Ahk
{
    public class MenuComparer : IComparer<Menu>
    {
        private readonly MenuState _state;

        public MenuComparer(MenuState state)
        {
            _state = state;
        }

        public int Compare(Menu x, Menu y)
        {
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

        private int CompareByLastAccessTime(Menu x, Menu y)
        {
            return y.LastAccess.CompareTo(x.LastAccess);
        }

        private bool FilterMatchesAtStart(Menu menu)
        {
            int index = menu.Name.IndexOf(_state.Filter, StringComparison.OrdinalIgnoreCase);
            return index == 0;
        }
    }
}