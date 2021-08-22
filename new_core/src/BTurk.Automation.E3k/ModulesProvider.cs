using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.E3k
{
    public class ModulesProvider : IRequestsProvider<Module>
    {
        public IEnumerable<Module> GetRequests()
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