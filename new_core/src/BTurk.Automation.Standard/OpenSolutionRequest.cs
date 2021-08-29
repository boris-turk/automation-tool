using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenSolutionRequest : Request, ICollectionRequest<Solution>
    {
        public OpenSolutionRequest() : base("solution")
        {
        }

        void ICollectionRequest<Solution>.OnLoaded(Solution solution)
        {
        }
    }
}