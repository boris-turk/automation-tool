﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BTurk.Automation.DependencyResolution;

[DebuggerStepThrough]
public class GenericMethodInvoker :
    GenericMethodInvoker.IMethod,
    GenericMethodInvoker.IWithGenericTypes,
    GenericMethodInvoker.IWithArguments
{
    private readonly object _instance;
    private readonly Type _hostingClassType;
    private string _methodName;
    private object[] _arguments;
    private Type[] _genericTypes;

    private GenericMethodInvoker(Type hostingClassType)
    {
        _hostingClassType = hostingClassType ?? throw new ArgumentNullException(nameof(hostingClassType));
        _arguments = new object[] { };
    }

    private GenericMethodInvoker(object instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        _hostingClassType = instance.GetType();
        _arguments = new object[] { };
    }

    public static IMethod Type(Type type)
    {
        return new GenericMethodInvoker(type);
    }

    public static IMethod Instance(object instance)
    {
        return new GenericMethodInvoker(instance);
    }

    public IWithGenericTypes Method(string methodName)
    {
        _methodName = methodName;
        return this;
    }

    public IWithArguments WithGenericTypes(params Type[] types)
    {
        _genericTypes = types;
        return this;
    }

    public IExecute WithArguments(params object[] arguments)
    {
        _arguments = arguments;
        return this;
    }

    public object Invoke()
    {
        var candidates = GetCandidates(_hostingClassType).ToList();

        if (candidates.Count > 1)
            candidates = candidates.Where(MatchesParameterTypes).ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException(
                $"Type ${_hostingClassType.FullName} contains no method " +
                $"with name {_methodName} that matches given constraints.");
        }

        if (candidates.Count > 1)
        {
            throw new InvalidOperationException(
                $"Type ${_hostingClassType.FullName} contains multiple " +
                $"methods named {_methodName} and it is impossible to " +
                "the correct one according to argument types.");
        }

        var method = candidates[0].MakeGenericMethod(_genericTypes);

        if (_instance == null)
            return method.Invoke(null, _arguments.ToArray());

        return method.Invoke(_instance, _arguments.ToArray());
    }

    private IEnumerable<MethodInfo> GetCandidates(Type type)
    {
        if (type == null)
            yield break;

        var flags = BindingFlags.NonPublic | BindingFlags.Public;

        if (_instance == null)
            flags = flags | BindingFlags.Static;
        else
            flags = flags | BindingFlags.Instance;

        foreach (var method in type.GetMethods(flags))
        {
            if (_arguments.Length != method.GetParameters().Length)
                continue;

            if (_genericTypes.Length != method.GetGenericArguments().Length)
                continue;

            if (method.Name.Equals(_methodName, StringComparison.Ordinal))
                yield return method;
        }

        foreach (var method in GetCandidates(type.BaseType))
            yield return method;
    }

    private bool MatchesParameterTypes(MethodInfo methodInfo)
    {
        var definitionTypes = methodInfo.GetParameters()
            .Select(x => x.ParameterType)
            .ToList();

        var outerTypes = _arguments
            .Select(x => x?.GetType())
            .ToList();

        if (outerTypes.Count != definitionTypes.Count)
            return false;

        for (int i = 0; i < outerTypes.Count; i++)
        {
            var outerType = outerTypes.ElementAt(i);
            var definitionType = definitionTypes.ElementAt(i);

            if (outerType == null || definitionType == null)
                continue;

            if (!outerType.InheritsFrom(definitionType))
                return false;
        }

        return true;
    }

    public interface IWithGenericTypes
    {
        IWithArguments WithGenericTypes(params Type[] types);
    }

    public interface IWithArguments : IExecute
    {
        IExecute WithArguments(params object[] arguments);
    }

    public interface IExecute
    {
        object Invoke();
    }

    public interface IMethod
    {
        IWithGenericTypes Method(string methodName);
    }
}