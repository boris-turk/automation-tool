using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable UnusedMember.Global

namespace AutomationEngine
{
	public class GenericMethodInvoker :
		GenericMethodInvoker.IMethod,
		GenericMethodInvoker.IWithGenericTypes,
		GenericMethodInvoker.IWithArguments
	{
		private readonly object _instance;
		private string _methodName;
		private object[] _arguments;
		private Type[] _genericTypes;

		private GenericMethodInvoker(object instance)
		{
			_instance = instance;
            _arguments = new object[] { };
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
            var candidates = GetCandidates(_instance.GetType()).ToList();

            if (candidates.Count > 1)
                candidates = candidates.Where(MatchesParameterTypes).ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Type ${_instance.GetType().FullName} contains no method " +
                    $"with name {_methodName} that matches given constraints.");
            }

            if (candidates.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Type ${_instance.GetType().FullName} contains multiple " +
                    $"methods named {_methodName} and it is impossible to " +
                    "the correct one according to argument types.");
            }

            var method = candidates[0].MakeGenericMethod(_genericTypes);

            return method.Invoke(_instance, _arguments.ToArray());
        }

        private IEnumerable<MethodInfo> GetCandidates(Type type)
        {
            if (type == null)
                yield break;

            var flags =
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public;

            foreach (var method in type.GetMethods(flags))
            {
                if (method.Name.Equals(_methodName, StringComparison.Ordinal))
                    yield return method;
            }

            foreach (var method in GetCandidates(type.BaseType))
                yield return method;
        }

        private bool MatchesParameterTypes(MethodInfo methodInfo)
        {
            if (!methodInfo.IsGenericMethod)
                return _genericTypes.Length == 0;

            var definitionTypes = methodInfo.GetGenericArguments().ToList();

            if (_genericTypes.Length != definitionTypes.Count)
                return false;

            for (int i = 0; i < _genericTypes.Length; i++)
            {
                var outerType = _genericTypes.ElementAt(i);
                var definitionType = definitionTypes.ElementAt(i);

                if (outerType == null || definitionType == null)
                    continue;

                if (!IsAssignableTo(outerType, definitionType))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsAssignableTo(Type type, Type candidateBaseType)
        {
            if (type.InheritsFrom(candidateBaseType))
                return true;

            if (!candidateBaseType.IsGenericType)
                return candidateBaseType.ContainsGenericParameters;

            if (candidateBaseType.IsGenericTypeDefinition)
                return false;

            var genericTypeDefinition = candidateBaseType.GetGenericTypeDefinition();

            return type.InheritsFrom(genericTypeDefinition);
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
}