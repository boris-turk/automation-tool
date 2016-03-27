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
            if (_state.Filter.Length == 0)
            {
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }
            if (FilterMatchesAtStart(x) && !FilterMatchesAtStart(y))
            {
                return -1;
            }
            if (!FilterMatchesAtStart(x) && FilterMatchesAtStart(y))
            {
                return 1;
            }
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        private bool FilterMatchesAtStart(BaseItem menu)
        {
            int index = menu.Name.IndexOf(_state.Filter, StringComparison.OrdinalIgnoreCase);
            return index == 0;
        }
    }
}
