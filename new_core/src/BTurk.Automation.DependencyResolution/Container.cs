using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using BTurk.Automation.Core;
using BTurk.Automation.Core.AsyncServices;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Core.Serialization;
using BTurk.Automation.Core.Views;
using BTurk.Automation.DependencyResolution.AsyncServices;
using BTurk.Automation.Standard;
using BTurk.Automation.WinForms;

namespace BTurk.Automation.DependencyResolution;

public static class Container
{
    private static readonly List<object> Singletons = new();

    private static readonly ClosedGenericTypeProvider ClosedGenericTypeProvider = new();

    static Container()
    {
        RegisterOpenGenericTypes();
    }

    public static T GetInstance<T>()
    {
        return (T)GetInstance(typeof(T));
    }

    public static object GetInstance(Type type)
    {
        if (IsSearchEngineRequest(type))
            return GetOrCreateSingleton<MainForm>(InitializeMainForm);

        if (type == typeof(IAsyncExecution))
            return GetOrCreateSingleton<AsyncExecutionDialog>();

        if (type == typeof(IAsyncExecutionDialog))
            return GetOrCreateSingleton<AsyncExecutionDialog>();

        if (type == typeof(IProcessStarter))
            return GetOrCreateSingleton<ProcessStarter>();

        if (type == typeof(IConfigurationProvider))
            return GetOrCreateSingleton<ConfigurationProvider>();

        if (type == typeof(IChildRequestsProvider))
            return GetOrCreateSingleton<ChildRequestsProvider>();

        if (type == typeof(IResourceProvider))
            return GetOrCreateSingleton<JsonResourceProvider>();

        if (type == typeof(IRequestActionDispatcher))
            return GetOrCreateSingleton<RequestActionDispatcher>();

        if (type == typeof(IRequestVisitor))
            return GetOrCreateSingleton<RequestVisitor>();

        if (type == typeof(IEnvironmentContextProvider))
            return GetOrCreateSingleton<EnvironmentContextProvider>();

        if (type == typeof(IMessagePublisher))
            return GetOrCreateSingleton<MessagePublisher>();

        if (type.InheritsFrom(typeof(ICommandProcessor)))
            return GetOrCreateSingleton<CommandProcessor>();

        if (type.InheritsFrom(typeof(IMessageHandler<>)))
            return GetOpenGenericServiceInstance(type, GetCompositeMessageHandlerType);

        if (type.InheritsFrom(typeof(IRequestsProvider<>)))
            return GetOpenGenericServiceInstance(type, GetRequestProviderType);

        if (type.InheritsFrom(typeof(ICommandHandler<>)))
            return GetOpenGenericServiceInstance(type, GetCommandHandlerType);

        if (type.InheritsFrom(typeof(IRequestActionDispatcher<>)))
            return GetOpenGenericServiceInstance(type, GetRequestActionDispatcherType);

        if (type.InheritsFrom(typeof(IRequestVisitor<,>)))
            return GetOpenGenericServiceInstance(type, GetRequestVisitorType);

        if (type.InheritsFrom(typeof(IEnumerable<>)))
            return GetEnumerableInstance(type);

        if (type == typeof(IViewProvider))
            return GetOrCreateSingleton<ViewProvider>();

        if (type == typeof(GlobalShortcuts))
            return GetOrCreateSingleton<GlobalShortcuts>();

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
            var argumentType = openGenericType.GetGenericArguments()[0];
            var implementorType = implementorTypeProvider.Invoke(argumentType);
            return CreateInstance(implementorType);
        }

        var instance = GetOrCreateSingleton(openGenericType, Create);

        var decoratorProducerParameters = GetDecoratorProducerParameters(openGenericType);

        if (decoratorProducerParameters == null)
            return instance;

        var producer = (Func<object, object>)
            GenericMethodInvoker.Type(typeof(Container))
                .Method(decoratorProducerParameters.MethodName)
                .WithGenericTypes(decoratorProducerParameters.Arguments)
                .Invoke();

        instance = producer.Invoke(instance);

        return instance;
    }

    private static object GetOpenGenericServiceInstance(
        Type openGenericType, Func<Type, Type, Type> implementorTypeProvider)
    {
        object Create()
        {
            var argumentType0 = openGenericType.GetGenericArguments()[0];
            var argumentType1 = openGenericType.GetGenericArguments()[1];
            var implementorType = implementorTypeProvider.Invoke(argumentType0, argumentType1);
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

    private static Type GetRequestProviderType(Type requestType)
    {
        return ClosedGenericTypeProvider.Get(typeof(IRequestsProvider<>), requestType);
    }

    private static Type GetRequestActionDispatcherType(Type requestType)
    {
        return typeof(RequestActionDispatcher<>).MakeGenericType(requestType);
    }

    private static Type GetRequestVisitorType(Type requestType, Type childRequestType)
    {
        if (requestType.InheritsFrom(typeof(SelectionRequest<>)))
        {
            var parentClosedGeneric = requestType.FindAllParentClosedGenerics(typeof(SelectionRequest<>)).Single();

            if (parentClosedGeneric.GetGenericArguments()[0] == childRequestType)
                return typeof(SelectionRequestVisitor<>).MakeGenericType(childRequestType);
        }
        else if (requestType.InheritsFrom(typeof(OptionRequest)))
        {
            return typeof(OptionRequestVisitor<>).MakeGenericType(childRequestType);
        }

        return typeof(DefaultRequestVisitor<,>).MakeGenericType(requestType, childRequestType);
    }

    private static Type GetCommandHandlerType(Type commandType)
    {
        return ClosedGenericTypeProvider.Get(typeof(ICommandHandler<>), commandType);
    }

    private static void InitializeMainForm(MainForm mainForm)
    {
        mainForm.RootMenuRequest = new RootMenuRequest();
        mainForm.Dispatcher = GetInstance<IRequestActionDispatcher>();
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

            var instance = Singletons.SingleOrDefault(s => s.GetType() == type);

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
        return new($"Failed to create instance of type {type.Name}");
    }

    private static T CreateInstance<T>()
    {
        return (T)CreateInstance(typeof(T));
    }

    private static object CreateInstance(Type type)
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var constructors = type.GetConstructors(bindingFlags).ToList();

        if (constructors.Count != 1)
        {
            throw new InvalidOperationException(
                $"Cannot create type {type.FullName} as it does not have exactly one public or internal constructor");
        }

        object[] parameters = (
            from parameter in constructors[0].GetParameters()
            let parameterInstance = GetInstance(parameter.ParameterType)
            select parameterInstance
        ).ToArray();

        var instance = Activator.CreateInstance(type, bindingFlags, binder: null, parameters, culture: null);
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

    private static void RegisterOpenGenericTypes()
    {
        ClosedGenericTypeProvider.Register(typeof(IRequestsProvider<>), typeof(EmptyRequestProvider<>));
        ClosedGenericTypeProvider.Register(typeof(ICommandHandler<>));
    }

    private static DecoratorProducerParameters GetDecoratorProducerParameters(Type serviceType)
    {
        if (!serviceType.InheritsFrom(typeof(ICommandHandler<>)))
            return null;

        var commandType = serviceType.GetGenericArguments().Single();

        if (!commandType.InheritsFrom(typeof(IAsyncCommand)))
            return null;

        return new DecoratorProducerParameters(
            nameof(GetAsyncCommandHandlerDecoratorProducer), commandType);
    }

    private static Func<object, object> GetAsyncCommandHandlerDecoratorProducer<TCommand>()
        where TCommand : IAsyncCommand
    {
        return h => new AsyncCommandHandlerDecorator<TCommand>(
            (ICommandHandler<TCommand>)h, GetInstance<IAsyncExecutionDialog>());
    }

    private class DecoratorProducerParameters
    {
        public DecoratorProducerParameters(string methodName, params Type[] arguments)
        {
            MethodName = methodName;
            Arguments = arguments;
        }

        public string MethodName { get; }
        public Type[] Arguments { get; }
    }
}