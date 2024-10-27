using System.Diagnostics;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.UnitTests;

[DebuggerDisplay("{" + nameof(DebuggerDisplayText) + "}")]
public class FakeRequest : Request
{
    public FakeRequest(string name, string text = null) : base(text)
    {
        Name = name;
    }

    public string Name { get; }

    private string DebuggerDisplayText
    {
        get
        {
            var displayText = string.IsNullOrWhiteSpace(Text) ? $"{Name}" : $"{Name}: {Text}";
            return displayText;
        }
    }

    public FakeRequest ExecutesCommand(ICommand command)
    {
        Configure().SetCommand(command);
        return this;
    }

    public FakeRequest WithChildren(params IRequestV2[] childRequests)
    {
        Configure().AddChildRequests(childRequests);
        return this;
    }

    public FakeRequest ScanChildrenIfUnmatched()
    {
        Configure().ScanChildrenIfUnmatched();
        return this;
    }
}