using System;
using System.Collections.Generic;

namespace AutomationEngine
{
    public class ExecutableItemComparer : IComparer<ExecutableItem>
    {
        public int Compare(ExecutableItem x, ExecutableItem y)
        {
            if (x.LastAccess != y.LastAccess)
            {
                return CompareByLastAccessTime(x, y);
            }

            return string.Compare(x.Name, y.Name, StringComparison.CurrentCulture);
        }

        private int CompareByLastAccessTime(ExecutableItem x, ExecutableItem y)
        {
            return y.LastAccess.CompareTo(x.LastAccess);
        }
    }
}
