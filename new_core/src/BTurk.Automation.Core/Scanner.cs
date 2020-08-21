using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;

namespace BTurk.Automation.Core
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
		}

        public void Setup(string executingDirectory)
        {
            LoadAllPlugins(AppDomain.CurrentDomain);
            _plugins.ForEach(p => p.Setup(executingDirectory));
        }

        public void Teardown()
        {
            _plugins.ForEach(p => p.Teardown());
        }

        public void Load(string name)
        {
            Assembly.Load(name);
        }
    }
}