using System;

// ReSharper disable UnusedTypeParameter

namespace BTurk.Automation.Core.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class IgnoreUnusedTypeWarningAttribute<T> : Attribute
{
}