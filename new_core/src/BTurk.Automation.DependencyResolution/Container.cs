using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Core.Serialization;

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

            if (type == typeof(RequestDispatcher))
                return GetOrCreateSingleton<RequestDispatcher>();

            throw FailedToCreateInstance(type);
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
            mainForm.RequestDispatcher = GetInstance<RequestDispatcher>();
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
