namespace BTurk.Automation.Standard
{
    public class CommitRepositoryRequest : RepositoryRequest
    {
        public CommitRepositoryRequest() : base("commit")
        {
        }

        protected override void OnRepositorySelected(Repository repository)
        {
            repository.Commit();
        }
    }
}
