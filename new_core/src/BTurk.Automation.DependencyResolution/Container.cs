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
            object instance = null;

            if (IsSearchEngineRequest(typeof(T)))
                instance = GetOrCreateMainForm();

            if (typeof(T) == typeof(ISearchHandler))
                instance = GetOrCreateSingleton(MainSearchHandler);

            if (instance == null)
                throw FailedToCreateInstance<T>();

            return (T)instance;
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
            void AttachSearchHandler(MainForm form) => form.SearchHandler = GetInstance<ISearchHandler>();
            return instance;
        }

        private static MainSearchHandler MainSearchHandler()
        {
            var handlers = GetOrCreateSingleton(SearchHandlers);
            var searchEngine = GetInstance<ISearchItemsProvider>();
            return new MainSearchHandler(searchEngine, handlers);
        }

        private static List<ISearchHandler> SearchHandlers()
        {
            return new List<ISearchHandler>
            {
                new CommitSearchHandler(),
                new FieldSearchHandler()
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

        private static InvalidOperationException FailedToCreateInstance<T>()
        {
            return new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}");
        }
    }
}
