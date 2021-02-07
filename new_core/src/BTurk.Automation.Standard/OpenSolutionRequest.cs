using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenSolutionRequest : Request, IRequestsConsumer<Solution>
    {
        private List<Solution> _solutions;

        public OpenSolutionRequest() : base("solution")
        {
        }

        public override IEnumerable<Request> ChildRequests()
        {
            return _solutions;
        }

        void IRequestsConsumer<Solution>.Add(IEnumerable<Solution> solutions)
        {
            _solutions = solutions.ToList();

            foreach (var solution in _solutions)
                solution.Action += () => solution.Open();
        }
    }
}