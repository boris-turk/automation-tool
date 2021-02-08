using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public abstract class RepositoryRequest : Request, IRequestConsumer<Repository>
    {
        protected RepositoryRequest(string text)
            : base(text)
        {
        }

        void IRequestConsumer<Repository>.Execute(Repository request)
        {
            OnRepositorySelected(request);
        }

        protected abstract void OnRepositorySelected(Repository request);
    }
}