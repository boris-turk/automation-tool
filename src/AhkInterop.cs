using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace AutomationEngine
{
    public static class AhkInterop
    {
        private static string MessageFile
        {
            get
            {
                string tempDir = System.Environment.GetEnvironmentVariable("TEMP");
                return Path.Combine(tempDir, @"_ahk_message_file.txt");
            }
        }

        public static IEnumerable<ExecutableItem> LoadRawFileContents(RawFileContentsSource source)
        {
            string[] arguments = new[]
            {
                source.Path,
                source.NameRegex.SearchRegex,
                source.NameRegex.Replacement,
                source.ReturnValueRegex.SearchRegex,
                source.ReturnValueRegex.Replacement,
            };

            using (var waitHandle = new ManualResetEvent(false))
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => waitHandle.Set();

                try
                {
                    MainForm.AhkFunctionResultReported += action;
                    ExecMethod("LoadRawFileContents", arguments.ToArray());
                    waitHandle.WaitOne();
                }
                finally
                {
                    MainForm.AhkFunctionResultReported -= action;
                }
            }

            List<string> result = GetMessageFileContents();

            for (int i = 0; i < result.Count; i += 2)
            {
                if (i >= result.Count - 1)
                {
                    break;
                }

                var executableItem = new ExecutableItem
                {
                    Name = result[i]
                };
                executableItem.Arguments.Add(new ExecutableItemArgument
                {
                    Type = ArgumentType.String,
                    Value = result[i + 1]
                });
                yield return executableItem;
            }
        }

        public static void ExecMethod(string name, params string[] arguments)
        {
            using (Process process = Process.GetProcessesByName("AutoHotKey").Single())
            {
                SerializeMethodInfo(name, arguments);
                MessageHelper.SendMessage(process, "func");
            }
        }

        private static void SerializeMethodInfo(string name, string[] arguments)
        {
            List<string> lines = arguments.ToList();
            lines.Insert(0, name);
            File.WriteAllLines(MessageFile, lines);
        }

        public static List<string> GetMessageFileContents()
        {
            return File.ReadAllLines(MessageFile).ToList();
        }
    }
}
