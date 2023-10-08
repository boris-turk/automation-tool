using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

[DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
public class SearchStep
{
    public SearchStep(IRequest request)
    {
        Request = request;
        Children = new List<IRequest>();
        Text = "";
    }

    public IRequest Request { get; }

    public List<IRequest> Children { get; }

    public string Text { get; set; }

    private string RequestTypeName => Request == null ? "" : Extensions.GetDebuggerDisplayText(Request);
}