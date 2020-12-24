using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Decorators;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Core.Serialization;
using BTurk.Automation.E3k;
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
            var instance = GetInstanceInternal(type);
            return new Decorators(instance, type).Apply();
        }

        public static object GetInstanceInternal(Type type)
        {
            if (IsSearchEngineRequest(type))
                return GetOrCreateSingleton<MainForm>(InitializeMainForm);

            if (type == typeof(IResourceProvider))
                return GetOrCreateSingleton<JsonResourceProvider>();

            if (type == typeof(IRequestHandler<CompositeRequest>))
                return GetOrCreateSingleton<CompositeRequestHandler>();

            if (type == typeof(RootRequestHandler))
                return GetOrCreateSingleton<RootRequestHandler>();

            if (type == typeof(IRequestHandler<CommandRequest>))
                return GetOrCreateSingleton<CommandRequestHandler>();

            if (type == typeof(IRequestHandler<SelectionRequest<Repository>>))
                return GetOrCreateSingleton(GetRepositoryRequestHandler);

            if (type == typeof(IRequestHandler<SelectionRequest<Solution>>))
                return GetOrCreateSingleton(GetSolutionRequestHandler);

            if (type == typeof(List<Command>))
                return GetOrCreateSingleton(Commands);

            if (type == typeof(CommandRequestHandler))
                return GetOrCreateSingleton<CommandRequestHandler>();

            throw FailedToCreateInstance(type);
        }

        private static IResourceProvider ResourceProvider => GetInstance<IResourceProvider>();

        private static ISearchEngine SearchEngine => GetInstance<ISearchEngine>();

        public static ISearchItemsProvider SearchItemsProvider => GetInstance<ISearchItemsProvider>();

        private static IRequestHandler<SelectionRequest<Repository>> GetRepositoryRequestHandler()
        {
            IRequestHandler<SelectionRequest<Repository>> handler = new RepositoryRequestHandler(SearchEngine);

            handler = new ClearSearchItemsRequestHandlerDecorator<SelectionRequest<Repository>>(handler, SearchItemsProvider);

            handler = new FilteredRequestHandlerDecorator<SelectionRequest<Repository>>(handler, SearchEngine);

            handler = new SelectionRequestHandlerDecorator<SelectionRequest<Repository>, Repository>(handler, SearchEngine);

            return handler;
        }

        private static IRequestHandler<SelectionRequest<Solution>> GetSolutionRequestHandler()
        {
            IRequestHandler<SelectionRequest<Solution>> handler = new SolutionSelectionRequestHandler(SearchEngine, ResourceProvider);

            handler = new ClearSearchItemsRequestHandlerDecorator<SelectionRequest<Solution>>(handler, SearchItemsProvider);

            handler = new FilteredRequestHandlerDecorator<SelectionRequest<Solution>>(handler, SearchEngine);

            handler = new SelectionRequestHandlerDecorator<SelectionRequest<Solution>, Solution>(handler, SearchEngine);

            return handler;
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

        private static void InitializeMainForm(MainForm mainForm)
        {
            mainForm.RootRequestHandler = GetInstance<RootRequestHandler>();
        }

        private static List<Command> Commands()
        {
            return new List<Command>
            {
                new CommitRequestHandler(),
                new SolutionRequestHandler(),
                new FieldRequestHandler()
            };
        }

        private static T GetOrCreateSingleton<T>(Action<T> initializer = null)
        {
            return GetOrCreateSingleton(CreateInstance<T>, initializer);
        }

        private static T GetOrCreateSingleton<T>(Func<T> provider, Action<T> initializer = null)
        {
            var lockTaken = false;

            try
            {
                Monitor.Enter(Singletons, ref lockTaken);

                var instance = (T)Singletons.SingleOrDefault(_ => _.GetType() == typeof(T));

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

            var parameters = (
                from parameter in constructors[0].GetParameters()
                let parameterInstance = GetInstance(parameter.ParameterType)
                select parameterInstance
            ).ToArray();

            var instance = Activator.CreateInstance(type, parameters);

            return instance;
        }
    }
}
