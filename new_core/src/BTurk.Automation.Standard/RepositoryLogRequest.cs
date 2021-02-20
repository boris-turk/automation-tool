namespace BTurk.Automation.Standard
{
    public class RepositoryLogRequest : RepositoryRequest
    {
        public RepositoryLogRequest() : base("log")
        {
        }

        public override void Execute(Repository repository)
        {
            repository.Log();
        }
    }
}