using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    public static class Container
    {
        private static readonly List<object> Singletons = new List<object>();

        public static T GetInstance<T>()
        {
            object instance = null;

            if (typeof(T) == typeof(MainForm))
                instance = GetOrCreateSingleton(CreateMainForm);

            if (typeof(T) == typeof(ISearchHandler) || typeof(T) == typeof(ISearchHandlersCollection))
                instance = GetOrCreateSingleton<MainSearchHandler>();

            if (instance == null)
                throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}");

            return (T)instance;
        }

        private static MainForm CreateMainForm()
        {
            return new MainForm
            {
                SearchHandler = GetInstance<ISearchHandler>()
            };
        }

        private static T GetOrCreateSingleton<T>() where T : new()
        {
            return GetOrCreateSingleton(() => new T());
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
