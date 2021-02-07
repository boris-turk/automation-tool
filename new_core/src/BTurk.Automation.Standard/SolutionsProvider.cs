using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class SolutionsProvider : RequestsProvider<Solution>
    {
        private readonly IResourceProvider _resourceProvider;

        public SolutionsProvider(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        protected override IEnumerable<Solution> Load()
        {
            return _resourceProvider.Load<List<Solution>>("solutions");
        }
    }
}