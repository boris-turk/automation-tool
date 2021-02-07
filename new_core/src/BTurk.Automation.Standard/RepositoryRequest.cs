using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public abstract class RepositoryRequest : Request, IRequestsConsumer<Repository>
    {
        private List<Repository> _repositories;

        protected RepositoryRequest(string text)
            : base(text)
        {
        }

        public void Add(IEnumerable<Repository> repositories)
        {
            _repositories = repositories.ToList();
            _repositories.ForEach(_ => _.Action = () => OnRepositorySelected(_));
        }

        protected abstract void OnRepositorySelected(Repository request);
    }
}