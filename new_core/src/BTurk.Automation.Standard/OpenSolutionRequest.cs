using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenSolutionRequest : CollectionRequest<Solution>
    {
        public OpenSolutionRequest() : base("solution")
        {
        }

        protected override void OnRequestLoaded(Solution solution)
        {
            solution.Command = new OpenWithDefaultProgramCommand(solution);
        }
    }
}