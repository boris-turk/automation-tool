using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace AutomationEngine
{
    internal class TimersCollection
    {
        private static readonly object LockObject = new object();

        public static TimersCollection Instance { get; } = new TimersCollection();

        // ReSharper disable once NotAccessedField.Local
        private Timer _timer;

        private List<ITimer> _timers;
        private bool _timerExecuting;

        public void LoadTimers()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            _timers = (
                    from file in new DirectoryInfo(directory).GetFiles()
                    where file.Extension.ToLower() == ".dll"
                    let assembly = Assembly.LoadFile(file.FullName)
                    from type in assembly.GetExportedTypes()
                    where typeof(ITimer).IsAssignableFrom(type)
                    select (ITimer)Activator.CreateInstance(type))
                .ToList();

            _timer = new Timer(state => OnTimerElapsed(), null, 0, 600000);
        }

        private void OnTimerElapsed()
        {
            lock (LockObject)
            {
                if (_timerExecuting)
                {
                    return;
                }
                _timerExecuting = true;
            }

            try
            {
                _timers.ForEach(t => t.Execute());
            }
            catch (Exception e)
            {
                string message = $"Error executing timers:{Environment.NewLine}{e}";
                MessageBox.Show(message);
            }
            finally
            {
                _timerExecuting = false;
            }
        }
    }
}