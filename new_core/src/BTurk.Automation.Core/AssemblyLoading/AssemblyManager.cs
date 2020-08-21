using System;
using System.IO;

// ReSharper disable LocalizableElement
// ReSharper disable AssignNullToNotNullAttribute

namespace BTurk.Automation.Core.AssemblyLoading
{
    public class AssemblyManager
    {
        private AppDomain _domain;
        private Scanner _scanner;

        public void LoadFrom(string[] paths)
        {
            if (_domain != null)
				Teardown();

            _domain = CreateAppDomain();
			_scanner = CreateScanner();

            try
            {
                foreach (var path in paths)
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    _scanner.Load(name);
                }
                _scanner.Setup(StartupProcess.CurrentAssemblyDirectory);
			}
			catch (Exception e)
	        {
				File.AppendAllText(@"ERROR_REPORT.txt", $"{e.Message}{Environment.NewLine}{e.StackTrace}");
				Teardown();
	        }
        }

        private static AppDomain CreateAppDomain()
        {
            var setup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Environment.CurrentDirectory,
                ShadowCopyFiles = "true",
                ShadowCopyDirectories = Environment.CurrentDirectory
            };

            var domainName = "BTurkAutomationDomain";
            var securityInfo = AppDomain.CurrentDomain.Evidence;
            return AppDomain.CreateDomain(domainName, securityInfo, setup);
        }

        private Scanner CreateScanner()
        {
            var type = typeof(Scanner);
            var assemblyFullName = type.Assembly.FullName;
            var typeFullName = type.FullName;
            var instance = _domain.CreateInstanceAndUnwrap(assemblyFullName, typeFullName);
            return (Scanner)instance;
        }

        private void Teardown()
	    {
			_scanner?.Teardown();
			AppDomain.Unload(_domain);
		}
    }
}