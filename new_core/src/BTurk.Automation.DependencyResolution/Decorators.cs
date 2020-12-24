using System;
using System.Linq;
using BTurk.Automation.Core.Decorators;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public class Decorators
    {
        private readonly object _instance;
        private readonly Type _requestedType;

        private Type[] _decorators => new[]
        {
            typeof(ClearSearchItemsRequestHandlerDecorator<>)
        };

        public Decorators(object instance, Type requestedType)
        {
            _instance = instance;
            _requestedType = requestedType;
        }

        public object Apply()
        {
            var instance = _instance;

            if (!_requestedType.IsGenericType)
                return instance;

            var arguments = _requestedType.GetGenericArguments();

            if (arguments.Length > 1)
                return instance;

            var argument = arguments.Single();

            if (argument.InheritsFrom(typeof(Request)))
            {
                instance = Invoke(nameof(ApplyRequestDecorators), argument, instance);
            }

            return instance;
        }

        private object Invoke(string methodName, Type genericArgumentType, object instance)
        {
            return GenericMethodInvoker.Instance(this)
                .Method(methodName)
                .WithGenericTypes(genericArgumentType)
                .WithArguments(instance)
                .Invoke();
        }

        public object ApplyRequestDecorators<T>(IRequestHandler<T> handler) where T : Request
        {
            return new ClearSearchItemsRequestHandlerDecorator<T>(
                handler, Container.GetInstance<ISearchItemsProvider>());
        }
    }
}