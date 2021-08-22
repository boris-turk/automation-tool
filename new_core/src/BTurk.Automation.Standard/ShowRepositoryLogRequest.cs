using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class ShowRepositoryLogRequest : Request, ICollectionRequest<Repository>
    {
        public ShowRepositoryLogRequest() : base("log")
        {
        }

        void ICollectionRequest<Repository>.OnLoaded(Repository repository)
        {
            repository.Command = new ShowRepositoryLogCommand(repository);
        }
    }
}