using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class CommitRepositoryRequest : Request, ICollectionRequest<Repository>
    {
        public CommitRepositoryRequest() : base("commit")
        {
        }

        void ICollectionRequest<Repository>.OnLoaded(Repository repository)
        {
            repository.Command = new CommitRepositoryCommand(repository);
        }
    }
}
