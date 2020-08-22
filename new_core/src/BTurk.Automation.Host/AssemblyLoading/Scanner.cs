using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using BTurk.Automation.Host.Plugins;
using BTurk.Automation.Host.SearchEngine;

namespace BTurk.Automation.Host.AssemblyLoading
{
    [Serializable]
    internal class Scanner : MarshalByRefObject
    {
        private readonly List<IPlugin> _plugins;

	    public Scanner()
        {
            _plugins = new List<IPlugin>();
        }

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override object InitializeLifetimeService()
		{
			return null;
		}

		private void LoadAllPlugins(AppDomain domain)
        {
			var instances = (
					from assembly in domain.GetAssemblies()
					from type in assembly.GetTypes()
					where type.GetInterface(typeof(IPlugin).Name) != null
					let constructor = type.GetConstructor(Type.EmptyTypes)
					where constructor != null
					select constructor.Invoke(null)
				)
				.Cast<IPlugin>()
				.ToList();

			_plugins.Clear();
			_plugins.AddRange(instances);

            if (!_plugins.Any())
                throw new InvalidOperationException("No plugin assemblies found");
		}

        public void Setup()
        {
            LoadAllPlugins(AppDomain.CurrentDomain);
            _plugins.ForEach(p => p.Setup());
        }

        public void Teardown()
        {
            _plugins.ForEach(p => p.Teardown());
        }

        public void Load(string name)
        {
            Assembly.Load(name);
        }

        public SearchResultsCollection Handle(SearchParameters parameters)
        {
            var handler = Bootstrapper.GetInstance<ISearchHandler>();
            return handler.Handle(parameters);
        }
    }
}
