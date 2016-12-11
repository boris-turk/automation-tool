using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutomationEngine
{
    internal class PluginsCollection
    {
        private static readonly PluginsCollection TheInstance = new PluginsCollection();
        private List<IPlugin> _plugins;

        public static PluginsCollection Instance => TheInstance;

        public void LoadPlugins()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            _plugins = (
                from file in new DirectoryInfo(directory).GetFiles()
                where file.Extension.ToLower() == ".dll"
                let assembly = Assembly.LoadFile(file.FullName)
                from type in assembly.GetExportedTypes()
                where typeof(IPlugin).IsAssignableFrom(type)
                select (IPlugin)Activator.CreateInstance(type))
                .ToList();
        }

        public void Execute(ExecutableItem item)
        {
            string[] arguments = item.Arguments.Select(x => x.InteropValue?.Trim('"')).ToArray();
            foreach (IPlugin plugin in _plugins.Where(x => x.Id == item.ExecutingMethodName))
            {
                plugin.Execute(arguments);
            }
        }
    }
}