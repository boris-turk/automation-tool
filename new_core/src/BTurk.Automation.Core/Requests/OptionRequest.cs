using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class OptionRequest : Request, ICollectionRequest
    {
        private readonly List<IRequest> _childRequests;

        public OptionRequest(params string[] options)
        {
            _childRequests = options.Select(_ => new Request(_)).Cast<IRequest>().ToList();

            ChildSelected = _ => SelectedIndex = _childRequests.IndexOf(_);
            ChildDeselected = _ => SelectedIndex = -1;
        }

        public int SelectedIndex { get; private set; }

        public string SelectedItem => _childRequests.ElementAtOrDefault(SelectedIndex)?.Text;

        public Action<IRequest> ChildSelected { get; }

        public Action<IRequest> ChildDeselected { get; }

        protected override bool CanAccept(DispatchPredicateContext context)
        {
            return context.ActionType == ActionType.Execute || context.ActionType == ActionType.MoveNext;
        }

        public IEnumerable<IRequest> GetRequests(EnvironmentContext context) => _childRequests;
    }
}