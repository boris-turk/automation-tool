using System;
using System.Collections.Generic;

namespace Ahk
{
    public class MenuItemComparator : IComparer<Menu>
    {
        public int Compare(Menu first, Menu second)
        {
            string x = first.Name;
            string y = second.Name;

            return String.Compare(x, y, StringComparison.Ordinal);
        }
    }
}