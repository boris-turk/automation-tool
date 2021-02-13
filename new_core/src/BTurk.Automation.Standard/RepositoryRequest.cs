using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public abstract class RepositoryRequest : Request, IRequestConsumer<Repository>
    {
        protected RepositoryRequest(string text)
            : base(text)
        {
        }

        public abstract void Execute(Repository request);
    }
}