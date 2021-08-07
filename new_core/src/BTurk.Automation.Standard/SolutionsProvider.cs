using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard
{
    public class SolutionsProvider : IRequestsProvider<Solution>
    {
        private readonly IResourceProvider _resourceProvider;

        public SolutionsProvider(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public virtual IEnumerable<Solution> Load()
        {
            return _resourceProvider.Load<List<Solution>>("solutions");
        }
    }
}