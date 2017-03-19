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

        public static PluginsCollection Instance => TheInstance;

        public void LoadPlugins()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            List<Type> types = (
                from file in new DirectoryInfo(directory).GetFiles()
                where file.Extension.ToLower() == ".dll"
                let assembly = Assembly.LoadFile(file.FullName)
                from type in assembly.GetExportedTypes()
                select type)
                .ToList();

            PluginLoaders = (
                from type in types
                where typeof(IPluginLoader).IsAssignableFrom(type)
                select (IPluginLoader)Activator.CreateInstance(type))
                .ToList();

            PluginExecutors = (
                from type in types
                where typeof(IPluginExecutor).IsAssignableFrom(type)
                select (IPluginExecutor)Activator.CreateInstance(type))
                .ToList();
        }

        public List<IPluginLoader> PluginLoaders { get; private set; }

        public List<IPluginExecutor> PluginExecutors { get; private set; }

        public void Execute(ExecutableItem item)
        {
            string[] arguments = item.Arguments.Select(x => x.InteropValue?.Trim('"')).ToArray();
            foreach (IPluginExecutor plugin in PluginExecutors.Where(x => x.Id == item.GetExecutingMethodName()))
            {
                plugin.Execute(arguments);
            }
        }
    }
}