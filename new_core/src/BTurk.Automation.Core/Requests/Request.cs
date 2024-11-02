using System.Diagnostics;
using System.Runtime.Serialization;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.Core.Requests;

[DataContract]
[DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
public class Request : IRequestV2
{
    private string _text;

    private readonly RequestConfigurationV2 _configuration;

    public Request()
    {
        _configuration = new RequestConfigurationV2();
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
    protected RequestConfigurationV2 Configure() => _configuration;

    IRequestConfigurationV2 IRequestV2.Configuration
    {
        [DebuggerStepThrough]
        get => _configuration;
    }
}