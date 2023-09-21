using System;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace BTurk.Automation.Host.AssemblyLoading
{
    [Serializable]
    internal class Scanner : MarshalByRefObject
    {
        private IGuestProcess _guestProcess;

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override object InitializeLifetimeService()
		{
			return null;
		}

		private void InitializeGuestProcessInstance(AppDomain domain)
        {
			var instances = (
					from assembly in domain.GetAssemblies()
					from type in assembly.GetTypes()
					where type.GetInterface(nameof(IGuestProcess)) != null
					let constructor = type.GetConstructor(Type.EmptyTypes)
					where constructor != null
					select constructor.Invoke(null)
				)
				.Cast<IGuestProcess>()
				.ToList();

            if (instances.Count == 0)
                throw new InvalidOperationException("No guest process defined.");

            if (instances.Count > 1)
            {
                var names = string.Join(",", instances.Select(p => p.GetType().Name));
                throw new InvalidOperationException($"More than one guest process defined: {names}");}

            _guestProcess = instances[0];
        }

        public void Setup()
        {
            InitializeGuestProcessInstance(AppDomain.CurrentDomain);
            ThreadPool.QueueUserWorkItem(_ => _guestProcess.Start());
        }

        public void Unload()
        {
            _guestProcess.Unload();
        }

        public void Load(string name)
        {
            Assembly.Load(name);
        }
    }
}
