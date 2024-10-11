using System.Diagnostics;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.UnitTests;

[DebuggerDisplay("{" + nameof(DebuggerDisplayText) + "}")]
public class FakeRequest : Request
{
    public FakeRequest(string friendlyName, string text = null) : base(text)
    {
        FriendlyName = friendlyName;
    }

    public string FriendlyName { get; }

    private string DebuggerDisplayText
    {
        get
        {
            var displayText = string.IsNullOrWhiteSpace(Text)
                ? $"{FriendlyName}"
                : $"{FriendlyName}: {Text}";

            return displayText;
        }
    }

    public FakeRequest WithChildren(params IRequestV2[] childRequests)
    {
        Configure().AddChildRequests(childRequests);
        return this;
    }
}