using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenSolutionRequest : Request, IRequestConsumer<Solution>
    {
        public OpenSolutionRequest() : base("solution")
        {
        }

        public void Execute(Solution solution)
        {
            solution.Open();
        }
    }
}