using System.Diagnostics;
using System.Runtime.Serialization;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.Core.Requests;

[DataContract]
[DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
public class Request : IRequest
{
    private string _text;

    private readonly RequestConfiguration _configuration;

    public Request()
    {
        _configuration = new RequestConfiguration();
    }

    public Request(string text) : this()
    {
        Text = text;
    }

    [DataMember(Name = "Text")]
    public virtual string Text
    {
        get => _text;
        set
        {
            _text = value;
            Configure().SetText(value);
        }
    }

    public override string ToString() => Text ?? "";

    private string RequestTypeName => Extensions.GetDebuggerDisplayText(this);

    [DebuggerStepThrough]
    protected RequestConfiguration Configure() => _configuration;

    IRequestConfiguration IRequest.Configuration
    {
        [DebuggerStepThrough]
        get => _configuration;
    }
}