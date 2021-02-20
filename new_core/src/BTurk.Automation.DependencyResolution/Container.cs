using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Core.Serialization;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public static class Container
    {
        private static readonly List<object> Singletons = new List<object>();

        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        public static object GetInstance(Type type)
        {
            if (IsSearchEngineRequest(type))
                return GetOrCreateSingleton<MainForm>(InitializeMainForm);

            if (type == typeof(IResourceProvider))
                return GetOrCreateSingleton<JsonResourceProvider>();

            if (type == typeof(IRequestVisitor))
                return GetOrCreateSingleton<RequestVisitDispatcher>();

            if (type == typeof(IEnvironmentContextProvider))
                return GetOrCreateSingleton<EnvironmentContextProvider>();

            if (type == typeof(IMessagePublisher))
                return GetOrCreateSingleton<MessagePublisher>();

            if (type.InheritsFrom(typeof(IMessageHandler<>)))
                return GetOpenGenericServiceInstance(type, GetCompositeMessageHandlerType);

            if (type.InheritsFrom(typeof(IRequestsProvider<>)))
                return GetOpenGenericServiceInstance(type, GetRequestProcessorType);

            if (type.InheritsFrom(typeof(IRequestExecutor<>)))
                return GetOpenGenericServiceInstance(type, GetRequestExecutorType);

            if (type.InheritsFrom(typeof(IRequestVisitor<>)))
                return GetOpenGenericServiceInstance(type, GetRequestVisitorType);

            if (type.InheritsFrom(typeof(IEnumerable<>)))
                return GetEnumerableInstance(type);

            throw FailedToCreateInstance(type);
        }

        private static object GetEnumerableInstance(Type enumerableType)
        {
            var serviceType = enumerableType.GetGenericArguments()[0];

            if (!serviceType.IsGenericType)
                return GetAllInstances(serviceType);

            var genericArguments = serviceType.GetGenericArguments();

            if (genericArguments.Length > 1)
            {
                throw new NotSupportedException(
                    "Creation of enumerable services with two generic arguments is not supported.");
            }

            var genericServiceTypeDefinition = serviceType.GetGenericTypeDefinition();
            var enumerable = GetAllInstances(genericServiceTypeDefinition, genericArguments[0]);

            var result = CastToProperEnumerable(enumerable, serviceType);

            return result;
        }

        private static object GetAllInstances(Type serviceType)
        {
            if (serviceType == typeof(IAdditionalEnvironmentDataProvider))
                return GetAllAdditionalEnvironmentDataProviders();

            return Activator.CreateInstance(typeof(List<>).MakeGenericType(serviceType));
        }

        private static IEnumerable GetAllInstances(Type openGenericServiceType, Type genericArgument)
        {
            if (openGenericServiceType == typeof(IMessageHandler<>))
                return GetAllMessageHandlers(genericArgument);

            return GetEmptyEnumerable(openGenericServiceType, genericArgument);
        }

        private static IEnumerable GetEmptyEnumerable(Type openGenericServiceType, Type genericArgument)
        {
            var serviceType = openGenericServiceType.MakeGenericType(genericArgument);
            var result = (IEnumerable)Activator.CreateInstance(typeof(List<>).MakeGenericType(serviceType));
            return result;
        }

        private static IEnumerable GetAllMessageHandlers(Type messageType)
        {
            if (messageType == typeof(ShowingAutomationWindowMessage))
                yield return GetOrCreateSingleton<EnvironmentContextProvider>();
        }

        private static IEnumerable<IAdditionalEnvironmentDataProvider> GetAllAdditionalEnvironmentDataProviders()
        {
            yield return GetOrCreateSingleton<VisualStudioEnvironmentDataProvider>();
        }

        private static Type GetCompositeMessageHandlerType(Type messageType)
        {
            return typeof(CompositeMessageHandler<>).MakeGenericType(messageType);
        }

        private static object GetOpenGenericServiceInstance(
            Type openGenericType, Func<Type, Type> implementorTypeProvider)
        {
            object Create()
            {
                var requestType = openGenericType.GetGenericArguments()[0];
                var implementorType = implementorTypeProvider.Invoke(requestType);
                return CreateInstance(implementorType);
            }

            var instance = GetOrCreateSingleton(openGenericType, Create);

            return instance;
        }

        private static bool IsSearchEngineRequest(Type type)
        {
            if (type == typeof(MainForm))
                return true;

            if (type == typeof(ISearchEngine))
                return true;

            if (type == typeof(ISearchItemsProvider))
                return true;

            return false;
        }

        private static Type GetRequestProcessorType(Type requestType)
        {
            if (requestType == typeof(Repository))
                return typeof(RepositoriesProvider);

            if (requestType == typeof(Solution))
                return typeof(SolutionsProvider);

            if (requestType == typeof(Note))
                return typeof(NotesProvider);

            return typeof(EmptyRequestProvider<>).MakeGenericType(requestType);
        }

        private static Type GetRequestVisitorType(Type requestType)
        {
            return typeof(RequestVisitor<>).MakeGenericType(requestType);
        }

        private static Type GetRequestExecutorType(Type requestType)
        {
            if (requestType == typeof(AhkSendRequest))
                return typeof(AhkSendRequestExecutor);

            return typeof(EmptyRequestExecutor<>).MakeGenericType(requestType);
        }

        private static void InitializeMainForm(MainForm mainForm)
        {
            mainForm.RequestVisitor = GetInstance<IRequestVisitor>();
            mainForm.EnvironmentContextProvider = GetInstance<IEnvironmentContextProvider>();
            mainForm.MessagePublisher = GetInstance<IMessagePublisher>();
        }

        private static T GetOrCreateSingleton<T>(Action<T> initializer = null)
        {
            return GetOrCreateSingleton(CreateInstance<T>, initializer);
        }

        private static T GetOrCreateSingleton<T>(Func<T> provider, Action<T> initializer = null)
        {
            void Initialize(object instance)
            {
                initializer?.Invoke((T)instance);
            }

            object Create()
            {
                return provider.Invoke();
            }

            var result = (T)GetOrCreateSingleton(typeof(T), Create, Initialize);

            return result;
        }

        private static object GetOrCreateSingleton(Type type, Func<object> provider, Action<object> initializer = null)
        {
            var lockTaken = false;

            try
            {
                Monitor.Enter(Singletons, ref lockTaken);

                var instance = Singletons.SingleOrDefault(_ => _.GetType() == type);

                if (instance == null)
                {
                    instance = provider.Invoke();
                    Singletons.Add(instance);
                    initializer?.Invoke(instance);
                }

                return instance;
            }
            finally
            {
                if (lockTaken) Monitor.Exit(Singletons);
            }
        }

        private static InvalidOperationException FailedToCreateInstance(Type type)
        {
            return new InvalidOperationException($"Failed to create instance of type {type.Name}");
        }

        private static T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        private static object CreateInstance(Type type)
        {
            var constructors = type.GetConstructors().Where(_ => _.IsPublic).ToList();

            if (constructors.Count != 1)
            {
                throw new InvalidOperationException(
                    $"Cannot create type {type.FullName} as it does not have exactly one public constructor");
            }

            object[] parameters = (
                from parameter in constructors[0].GetParameters()
                let parameterInstance = GetInstance(parameter.ParameterType)
                select parameterInstance
            ).ToArray();

            var instance = Activator.CreateInstance(type, parameters);
            return instance;
        }

        private static object CastToProperEnumerable(object instance, Type targetType)
        {
            return GenericMethodInvoker.Type(typeof(Container))
                .Method(nameof(CastToProperEnumerable))
                .WithGenericTypes(targetType)
                .WithArguments(instance)
                .Invoke();
        }

        private static IEnumerable<TItem> CastToProperEnumerable<TItem>(IEnumerable instance)
        {
            return instance.Cast<TItem>();
        }
    }
}
