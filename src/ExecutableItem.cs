using System;
using System.Collections.Generic;

namespace Ahk
{
    public class ExecutableItem
    {
        public ExecutableItem()
        {
            Arguments = new List<ExecutableItemArgument>();
        }

        public string Name { get; set; }

        public List<ExecutableItemArgument> Arguments { get; set; }

        public DateTime LastAccess { get; set; }
    }
}