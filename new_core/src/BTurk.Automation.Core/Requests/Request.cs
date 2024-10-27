using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.Core.Requests;

[DataContract]
[DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
public class Request : IRequest
{
    private string _text;

    private readonly RequestConfigurationV2 _configuration;

    public static readonly Request Null = new();

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

    public ICommand Command { get; set; }

    public Predicate<DispatchPredicateContext> CanAcceptPredicate { get; set; }

    public override string ToString() => Text ?? "";

    protected virtual bool CanAccept(DispatchPredicateContext context)
    {
        if (context.ActionType == ActionType.Search)
            return context.Text.Trim().Length > 0 && context.Text.EndsWith(" ");

        return context.ActionType == ActionType.Execute;
    }

    private string RequestTypeName => Extensions.GetDebuggerDisplayText(this);

    bool IRequest.CanAccept(DispatchPredicateContext context)
    {
        if (CanAcceptPredicate != null)
            return CanAcceptPredicate.Invoke(context);

        return CanAccept(context);
    }

    [DebuggerStepThrough]
    protected RequestConfigurationV2 Configure() => _configuration;

    IRequestConfigurationV2 IRequestV2.Configuration
    {
        [DebuggerStepThrough]
        get => _configuration;
    }
}