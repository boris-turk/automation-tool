using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable RedundantNameQualifier

namespace BTurk.Automation.DependencyResolution
{
    public class ClosedGenericTypeProvider
    {
        private Assembly[] Assemblies { get; } = CollectAssemblies();

        private static Dictionary<Type, ClosedGenericTypes> ClosedGenericTypesCollection { get; } = new();

        private static Assembly[] CollectAssemblies()
        {
            return new[]
            {
                typeof(global::BTurk.Automation.Core.Requests.Request).Assembly,
                typeof(global::BTurk.Automation.DependencyResolution.Container).Assembly,
                typeof(global::BTurk.Automation.E3k.Module).Assembly,
                typeof(global::BTurk.Automation.Mic.Salon).Assembly,
                typeof(global::BTurk.Automation.Standard.Note).Assembly,
            };
        }

        public Type Get(Type openGenericType, Type argumentType)
        {
            if (!ClosedGenericTypesCollection.TryGetValue(openGenericType, out var collection))
                throw new Exception($"No registration for open generic service {openGenericType.Name}");

            return collection.Get(argumentType);
        }

        public void Register(Type openGenericServiceType, Type fallbackOpenGenericServiceType)
        {
            if (!openGenericServiceType.IsGenericTypeDefinition)
                throw new Exception($"Not a generic type definition: {openGenericServiceType.FullName}");

            var types = (
                from assembly in Assemblies
                from type in assembly.GetExportedTypes()
                where type.FindAllParentClosedGenerics(openGenericServiceType).Any()
                select type
            ).ToList();

            ClosedGenericTypesCollection[openGenericServiceType] =
                new ClosedGenericTypes(openGenericServiceType, types, fallbackOpenGenericServiceType);
        }

        private class ClosedGenericTypes
        {
            private readonly List<Type> _types;

            private readonly Type _openGenericType;

            private readonly Type _fallbackOpenGenericType;

            public ClosedGenericTypes(Type openGenericType, List<Type> types, Type fallbackOpenGenericType)
            {
                _types = types;
                _openGenericType = openGenericType;
                _fallbackOpenGenericType = fallbackOpenGenericType;
            }

            public Type Get(Type argumentType)
            {
                var candidate = _types.FirstOrDefault(_ => _
                    .FindAllParentClosedGenerics(_openGenericType)
                    .Any(p => p.GetGenericArguments()[0] == argumentType)
                );

                if (candidate != null)
                    return candidate;

                return _fallbackOpenGenericType.MakeGenericType(argumentType);
            }
        }
    }
}