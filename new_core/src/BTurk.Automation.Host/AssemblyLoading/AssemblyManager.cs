using System;
using System.IO;

// ReSharper disable LocalizableElement
// ReSharper disable AssignNullToNotNullAttribute

namespace BTurk.Automation.Host.AssemblyLoading
{
    public class AssemblyManager
    {
        private AppDomain _domain;
        private Scanner _scanner;

        private string[] GetAssemblyPaths()
        {
            return Directory.GetFiles(StartupProcess.CurrentAssemblyDirectory, "*.dll");
        }

        public void Load()
        {
            Unload();

            _domain = CreateAppDomain();
			_scanner = CreateScanner();

            try
            {
                foreach (var path in GetAssemblyPaths())
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    _scanner.Load(name);
                }

                _scanner.Setup();
			}
			catch (Exception e)
	        {
				File.AppendAllText(@"ERROR_REPORT.txt", $"{e.Message}{Environment.NewLine}{e.StackTrace}");
				Unload();
	        }
        }

        public void Unload()
        {
            if (_domain == null)
                return;

            _scanner?.Unload();
            _scanner = null;

            AppDomain.Unload(_domain);
            _domain = null;
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
    }
}
