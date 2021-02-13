namespace BTurk.Automation.Standard
{
    public class CommitRepositoryRequest : RepositoryRequest
    {
        public CommitRepositoryRequest() : base("commit")
        {
        }

        public override void Execute(Repository repository)
        {
            repository.Commit();
        }
    }
}
