using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class OptionRequest : CollectionRequest<IRequest>
{
    private readonly List<IRequest> _childRequests;

    public OptionRequest(params string[] options)
    {
        _childRequests = options.Select(o => new Request(o)).Cast<IRequest>().ToList();

        ChildSelected = r => SelectedIndex = _childRequests.IndexOf(r);
        ChildDeselected = _ => SelectedIndex = -1;
    }

    public int SelectedIndex { get; private set; }

    public string SelectedItem => _childRequests.ElementAtOrDefault(SelectedIndex)?.Text;

    public Action<IRequest> ChildSelected { get; }

    public Action<IRequest> ChildDeselected { get; }

    protected override bool CanAccept(DispatchPredicateContext context)
    {
        return context.ActionType is ActionType.Execute or ActionType.Search;
    }

    protected override IEnumerable<IRequest> GetRequests() => _childRequests;
}