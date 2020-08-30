﻿using System;
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
                return GetOrCreateMainForm();

            if (type == typeof(IRequestHandler<CompositeRequest>))
                return GetOrCreateSingleton(CompositeRequestHandler);

            if (type == typeof(IRequestHandler<Request>))
                return GetOrCreateSingleton(MainRequestHandler);

            if (type == typeof(IRequestHandler<RootCommandRequest>))
                return GetOrCreateSingleton(CreateRootCommandRequestHandler);

            if (type == typeof(IRequestHandler<SelectionRequest<Repository>>))
                return GetOrCreateSingleton(GetRepositoryRequestHandler);

            if (instance == null)
                throw FailedToCreateInstance(type);

            return instance;
        }

        private static ISearchEngine SearchEngine => GetInstance<ISearchEngine>();

        public static ISearchItemsProvider SearchItemsProvider => GetInstance<ISearchItemsProvider>();

        private static RepositoryRequestHandler GetRepositoryRequestHandler()
        {
            return new RepositoryRequestHandler(GetInstance<ISearchItemsProvider>(), SearchEngine);
        }

        private static IRequestHandler<RootCommandRequest> CreateRootCommandRequestHandler()
        {
            var handler = new RootCommandRequestHandler(SearchEngine);

            return new ClearSearchItemsRequestHandlerDecorator<RootCommandRequest>(
                handler, SearchEngine, SearchItemsProvider);
        }

        private static IRequestHandler<CompositeRequest> CompositeRequestHandler()
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
            void AttachSearchHandler(MainForm form) => form.RequestHandler = GetInstance<IRequestHandler<Request>>();
            return instance;
        }

        private static MainRequestHandler MainRequestHandler()
        {
            var handlers = GetOrCreateSingleton(RootRequestHandlers);
            var searchEngine = GetInstance<ISearchItemsProvider>();
            return new MainRequestHandler(searchEngine, handlers);
        }

        private static List<IRequestHandler<Request>> RootRequestHandlers()
        {
            return new List<IRequestHandler<Request>>
            {
                new CommitRequestHandler(GetInstance<IRequestHandler<CompositeRequest>>()),
                new FieldRequestHandler(GetInstance<IRequestHandler<CompositeRequest>>())
            };
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
    }
}
