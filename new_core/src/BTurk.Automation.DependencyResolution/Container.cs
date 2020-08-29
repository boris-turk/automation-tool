using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BTurk.Automation.Core.SearchEngine;
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
            object instance = null;

            if (IsSearchEngineRequest(type))
                instance = GetOrCreateMainForm();

            if (type == typeof(ISearchHandler<CompositeRequest>))
                instance = GetOrCreateSingleton(CompositeRequestHandler);

            if (type == typeof(ISearchHandler<Request>))
                instance = GetOrCreateSingleton(MainSearchHandler);

            if (type == typeof(ISearchHandler<RootCommandRequest>))
                instance = GetOrCreateSingleton(CreateRootCommandRequestHandler);

            if (instance == null)
                throw FailedToCreateInstance(type);

            return instance;
        }

        private static ISearchHandler<RootCommandRequest> CreateRootCommandRequestHandler()
        {
            return new RootCommandRequestHandler(GetInstance<ISearchEngine>());
        }

        private static ISearchHandler<CompositeRequest> CompositeRequestHandler()
        {
            return new CompositeRequestHandler();
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

        private static MainForm GetOrCreateMainForm()
        {
            var instance = GetOrCreateSingleton(() => new MainForm(), AttachSearchHandler);
            void AttachSearchHandler(MainForm form) => form.SearchHandler = GetInstance<ISearchHandler<Request>>();
            return instance;
        }

        private static MainSearchHandler MainSearchHandler()
        {
            var handlers = GetOrCreateSingleton(SearchHandlers);
            var searchEngine = GetInstance<ISearchItemsProvider>();
            return new MainSearchHandler(searchEngine, handlers);
        }

        private static List<ISearchHandler<Request>> SearchHandlers()
        {
            return new List<ISearchHandler<Request>>
            {
                new CommitSearchHandler(GetInstance<ISearchHandler<CompositeRequest>>()),
                new FieldSearchHandler(GetInstance<ISearchHandler<CompositeRequest>>())
            };
        }

        private static T GetOrCreateSingleton<T>(Func<T> provider, Action<T> initializer = null)
        {
            var lockTaken = false;

            try
            {
                Monitor.Enter(Singletons, ref lockTaken);

                var instance = Singletons.OfType<T>().SingleOrDefault();

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
    }
}
