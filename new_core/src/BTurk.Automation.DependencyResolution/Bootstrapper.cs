using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BTurk.Automation.Core;
using BTurk.Automation.Core.AsyncServices;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Configuration;
using BTurk.Automation.Core.Converters;
using BTurk.Automation.Core.DataPersistence;
using BTurk.Automation.Core.FileSystem;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Presenters;
using BTurk.Automation.Core.Queries;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Core.Serialization;
using BTurk.Automation.Core.Views;
using BTurk.Automation.DependencyResolution.AsyncServices;
using BTurk.Automation.Standard;
using BTurk.Automation.WinForms;
using BTurk.Automation.WinForms.Controls;
using BTurk.Automation.WinForms.Providers;
using SimpleInjector;

// ReSharper disable RedundantNameQualifier

namespace BTurk.Automation.DependencyResolution;

public class Bootstrapper
{
    public static Container Container { get; private set; }

    private Assembly[] Assemblies { get; } = {
        typeof(global::BTurk.Automation.Core.Requests.Request).Assembly,
        typeof(global::BTurk.Automation.DependencyResolution.Bootstrapper).Assembly,
        typeof(global::BTurk.Automation.E3k.Module).Assembly,
        typeof(global::BTurk.Automation.Mic.Salon).Assembly,
        typeof(global::BTurk.Automation.Standard.Note).Assembly,
        typeof(global::BTurk.Automation.WinForms.Controls.CustomForm).Assembly,
    };

    public static void InitializeContainer()
    {
        new Bootstrapper().InstallRegistrations();
    }

    private void InstallRegistrations()
    {
        Container = new Container();

        Container.RegisterSingleton<ISearchEngineV2, MainForm>();
        Container.RegisterSingleton<IChildRequestsProviderV2, ChildRequestsProvider>();
        Container.RegisterSingleton<IRequestActionDispatcherV2, RequestActionDispatcherV2>();

        Container.RegisterSingleton<MainForm>();
        Container.RegisterSingleton<ISearchEngine, MainForm>();
        Container.RegisterSingleton<ISearchItemsProvider, MainForm>();
        Container.RegisterInitializer<MainForm>(InitializeMainForm);
        Container.RegisterSingleton<IAsyncExecution, AsyncExecutionDialog>();
        Container.RegisterSingleton<IAsyncExecutionDialog, AsyncExecutionDialog>();
        Container.RegisterSingleton<IProcessStarter, ProcessStarter>();
        Container.RegisterSingleton<IConfigurationProvider, ConfigurationProvider>();
        Container.RegisterSingleton<IChildRequestsProvider, ChildRequestsProvider>();
        Container.RegisterSingleton<IResourceProvider, JsonResourceProvider>();
        Container.RegisterSingleton<IRequestActionDispatcher, RequestActionDispatcher>();
        Container.RegisterSingleton<IRequestVisitor, RequestVisitor>();
        Container.RegisterSingleton<IEnvironmentContextProvider, EnvironmentContextProvider>();
        Container.RegisterSingleton<IMessagePublisher, MessagePublisher>();
        Container.RegisterSingleton<IViewProvider, ViewProvider>();
        Container.RegisterSingleton<GlobalShortcuts, GlobalShortcuts>();
        Container.RegisterSingleton<IQueryProcessor, QueryProcessor>();
        Container.RegisterSingleton<ICommandProcessor, CommandProcessor>();
        Container.RegisterSingleton<IControlProvider, ControlProvider>();
        Container.RegisterSingleton<IGuiValueConverter, GuiValueConverter>();
        Container.RegisterSingleton<IDirectoryProvider, DirectoryProvider>();

        RegisterConcreteInheritors<IPresenter>(Lifestyle.Transient);

        Container.RegisterDecorator(typeof(ICommandHandler<>), typeof(AsyncCommandHandlerDecorator<>));

        RegisterGenericServiceImplementations(typeof(ICommandHandler<>), Lifestyle.Singleton,
            excluded: typeof(AsyncCommandHandlerDecorator<>));

        RegisterGenericServiceImplementations(typeof(IQueryHandler<,>), Lifestyle.Singleton);

        RegisterGenericServiceImplementations(typeof(IRequestsProvider<>), Lifestyle.Singleton,
            excluded: typeof(EmptyRequestProvider<>));

        RegisterGenericServiceImplementations(typeof(IRequestActionDispatcher<>), Lifestyle.Singleton);
        RegisterGenericServiceImplementations(typeof(IRequestVisitor<,>), Lifestyle.Singleton,
            excluded: typeof(DefaultRequestVisitor<,>));

        RegisterGenericServiceImplementations(typeof(IControlProvider<>), Lifestyle.Singleton);
        RegisterGenericServiceImplementations(typeof(IGuiValueConverter<,>), Lifestyle.Singleton);

        RegisterCollectionServiceImplementations(typeof(IAdditionalEnvironmentDataProvider), Lifestyle.Singleton);

        Container.RegisterConditional(typeof(IMessageHandler<>), typeof(CompositeMessageHandler<>), c => !c.Handled);

        RegisterCollectionServiceImplementations(typeof(IMessageHandler<>), Lifestyle.Singleton,
            excluded: typeof(CompositeMessageHandler<>));

        Container.RegisterConditional(typeof(IRequestsProvider<>), typeof(EmptyRequestProvider<>),
            Lifestyle.Singleton, c => !c.Handled);

        Container.RegisterConditional(typeof(IRequestVisitor<,>), typeof(DefaultRequestVisitor<,>),
            Lifestyle.Singleton, c => !c.Handled);
    }

    private void RegisterConcreteInheritors<TParent>(Lifestyle lifestyle)
    {
        var types = (
            from assembly in Assemblies
            from type in assembly.GetExportedTypes()
            where type.InheritsFrom(typeof(TParent))
            where !type.IsAbstract
            select type
        ).ToList();

        foreach (var type in types)
            Container.Register(type, type, lifestyle);
    }

    private void InitializeMainForm(MainForm mainForm)
    {
        mainForm.RootMenuRequest = new RootMenuRequest();
        mainForm.Dispatcher = Container.GetInstance<IRequestActionDispatcher>();
        mainForm.EnvironmentContextProvider = Container.GetInstance<IEnvironmentContextProvider>();
        mainForm.MessagePublisher = Container.GetInstance<IMessagePublisher>();
    }

    private void RegisterGenericServiceImplementations(Type serviceType, Lifestyle lifestyle, params Type[] excluded)
    {
        var implementorTypes = GetImplementorTypes(serviceType);

        foreach (var implementorType in implementorTypes)
        {
            var parentClosedGenericServiceTypes = new List<Type> { serviceType };

            if (excluded != null && excluded.Contains(implementorType))
                continue;

            if (serviceType.IsGenericTypeDefinition && !implementorType.IsGenericTypeDefinition)
                parentClosedGenericServiceTypes = implementorType.FindAllParentClosedGenerics(serviceType).ToList();

            foreach (var closedGenericServiceType in parentClosedGenericServiceTypes)
                Container.Register(closedGenericServiceType, implementorType, lifestyle);
        }
    }

    private void RegisterCollectionServiceImplementations(Type serviceType, Lifestyle lifestyle, params Type[] excluded)
    {
        var implementorTypes = GetImplementorTypes(serviceType).ToList();

        foreach (var implementorType in implementorTypes)
        {
            if (excluded != null && excluded.Contains(implementorType))
                continue;

            var parentClosedGenericServiceTypes = serviceType.IsGenericType
                ? implementorType.FindAllParentClosedGenerics(serviceType)
                : new[] { serviceType };

            foreach (var closedGenericServiceType in parentClosedGenericServiceTypes)
                Container.Collection.Append(closedGenericServiceType, implementorType, lifestyle);
        }
    }

    private IEnumerable<Type> GetImplementorTypes(Type serviceType)
    {
        var options = new TypesToRegisterOptions
        {
            IncludeComposites = false,
            IncludeDecorators = false,
            IncludeGenericTypeDefinitions = true
        };

        var implementorTypes = Container.GetTypesToRegister(serviceType, Assemblies, options);

        return implementorTypes;
    }
}
