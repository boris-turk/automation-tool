﻿using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Configuration;

// ReSharper disable LocalizableElement

namespace BTurk.Automation.DependencyResolution;

public static class Extensions
{
    public static string GetKeePassDatabase(this SystemConfiguration configuration)
    {
        return configuration.GetFilePath("KeePassDatabase");
    }

    public static string GetFriendlyName(this Type type)
    {
        var typeArguments = "";

        if (type.IsGenericType)
            typeArguments = string.Join(", ", type.GetGenericArguments().Select(t => t.Name));

        if (typeArguments.HasLength())
            return $"{type.Name}<{typeArguments}>";

        return type.Name;
    }

    public static bool InheritsFrom(this Type type, Type parent)
    {
        if (parent.IsAssignableFrom(type))
        {
            return true;
        }

        while (type != null && type != typeof(object))
        {
            Type current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (parent == current)
            {
                return true;
            }
            if (parent.IsInterface)
            {
                var interfaces = type.GetInterfaces().Where(x => x.IsGenericType);
                return interfaces.Any(x => x.GetGenericTypeDefinition() == parent);
            }
            type = type.BaseType;
        }
        return false;
    }

    public static IEnumerable<Type> FindAllParentClosedGenerics(this Type type, Type openGenericType)
    {
        if (!openGenericType.IsGenericTypeDefinition)
        {
            throw new ArgumentException("Argument is not an open generic type", nameof(openGenericType));
        }

        if (openGenericType.IsInterface)
        {
            return
                from parentType in type.GetInterfaces()
                where parentType.IsGenericType
                let genericType = parentType.GetGenericTypeDefinition()
                where genericType == openGenericType
                select parentType;
        }

        var parent = type;

        while (parent != null)
        {
            if (parent.IsGenericType && parent.GetGenericTypeDefinition() == openGenericType)
            {
                return new[] { parent };
            }
            parent = parent.BaseType;
        }

        return new Type[] { };
    }
}