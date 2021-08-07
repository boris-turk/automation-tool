using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.E3k
{
    public class ModulesProvider : IRequestsProvider<Module>
    {
        public IEnumerable<Module> Load()
        {
            var modules = new Module[]
            {
                new(2, "Addresses"),
                new(3, "Inventory"),
            };

            return modules;
        }
    }
}