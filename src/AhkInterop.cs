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
        private const string VoidReturnType = "Void";

        private static string MessageFile
        {
            get
            {
                string tempDir = Environment.GetEnvironmentVariable("TEMP");
                return Path.Combine(tempDir, @"_ahk_message_file.txt");
            }
        }

        public static IEnumerable<ExecutableItem> ExecuteFunction(AhkContentSource source)
        {
            using (var waitHandle = new ManualResetEvent(false))
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => waitHandle.Set();

                var mainForm = FormFactory.Instance<MainForm>();

                try
                {
                    mainForm.AhkFunctionResultReported += action;
                    ExecuteMethod(source.ReturnType, source.Function, source.InteropArguments.ToArray());
                    waitHandle.WaitOne();
                }
                finally
                {
                    mainForm.AhkFunctionResultReported -= action;
                }
            }

            List<string> result = GetMessageFileContents();

            DateTime timeStamp = DateTime.MaxValue;
            for (int i = 0; i < result.Count; i += 2)
            {
                if (i >= result.Count - 1)
                {
                    break;
                }

                timeStamp = timeStamp.AddTicks(-1);

                ExecutableItem executableItem;
                if (source.ReturnsFilePaths)
                {
                    executableItem = new FileItem();
                }
                else
                {
                    executableItem = new ExecutableItem();
                }

                executableItem.Name = result[i];
                executableItem.LastAccess = timeStamp;

                executableItem.Arguments.Add(new StringValue
                {
                    Value = result[i + 1]
                });
                yield return executableItem;
            }
        }

        public static void ExecuteMethod(string name, params AbstractValue[] arguments)
        {
            ExecuteMethod(VoidReturnType, name, arguments);
        }

        private static void ExecuteMethod(string returnType, string name, params AbstractValue[] arguments)
        {
            string[] properArguments = arguments.Select(x => x.InteropValue).ToArray();

            using (Process process = Process.GetProcessesByName("AutoHotKey").Single())
            {
                SerializeMethodInfo(returnType, name, properArguments);
                MessageHelper.SendMessage(process, "func");
            }
        }

        private static void SerializeMethodInfo(string returnType, string name, string[] arguments)
        {
            List<string> lines = arguments.ToList();
            lines.Insert(0, returnType);
            lines.Insert(1, name);
            File.WriteAllLines(MessageFile, lines);
        }

        public static List<string> GetMessageFileContents()
        {
            return File.ReadAllLines(MessageFile).ToList();
        }
    }
}
