using System.Diagnostics;
using System.Runtime.Serialization;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.Core.Requests;

[DataContract]
[DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
public class Request : IRequest
{
    private readonly RequestConfiguration _configuration = new();

    [DataMember(Name = "Text")]
    public string Text
    {
        get => ((IRequestConfiguration)_configuration).Text;
        set => Configure().SetText(value);
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