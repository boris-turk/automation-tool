﻿using System.Diagnostics;
using BTurk.Automation.Core.Requests;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

[DebuggerStepThrough]
public class RequestVisitor : IRequestVisitor
{
    public RequestVisitor(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

    void IRequestVisitor.Visit(RequestVisitContext context)
    {
        GenericMethodInvoker.Instance(this)
            .Method(nameof(Visit))
            .WithGenericTypes(context.Request.GetType(), context.ChildRequest.GetType())
            .WithArguments(context)
            .Invoke();
    }

    public void Visit<TRequest, TChild>(RequestVisitContext context)
        where TRequest : IRequest
        where TChild : IRequest
    {
        var properContext = context.CastTo<TRequest, TChild>();
        var visitor = Container.GetInstance<IRequestVisitor<TRequest, TChild>>();
        visitor.Visit(properContext);
    }
}