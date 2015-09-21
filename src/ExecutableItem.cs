using System;

namespace Ahk
{
    public class ExecutableItem
    {
        public const string Separator = "-||-";

        public ExecutableItem(string definition)
        {
            string[] entries = definition.Split(new[] { Separator },
                StringSplitOptions.RemoveEmptyEntries);

            if (entries.Length > 1)
            {
                Name = entries[0].Trim();
                Parameter = entries[1].Trim();
            }
        }

        public string Name { get; set; }

        public string Parameter { get; set; }
    }
}