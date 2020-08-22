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

            if (typeof(T) == typeof(MainForm))
                instance = GetOrCreateSingleton(MainForm);

            if (typeof(T) == typeof(ISearchHandler))
                instance = GetOrCreateSingleton(MainSearchHandler);

            if (instance == null)
                throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}");

            return (T)instance;
        }

        private static MainSearchHandler MainSearchHandler()
        {
            var handlers = GetOrCreateSingleton(SearchHandlers);
            return new MainSearchHandler(handlers);
        }

        private static List<ISearchHandler> SearchHandlers()
        {
            return new List<ISearchHandler>
            {
                new CommitSearchHandler(),
                new FieldSearchHandler()
            };
        }

        private static MainForm MainForm()
        {
            return new MainForm
            {
                SearchHandler = GetInstance<ISearchHandler>()
            };
        }

        private static T GetOrCreateSingleton<T>(Func<T> provider)
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
                }

                return instance;
            }
            finally
            {
                if (lockTaken) Monitor.Exit(Singletons);
            }
        }
    }
}
