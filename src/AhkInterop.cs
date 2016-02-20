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
                string tempDir = Environment.GetEnvironmentVariable("TEMP");
                return Path.Combine(tempDir, @"_ahk_message_file.txt");
            }
        }

        public static IEnumerable<ExecutableItem> LoadRawFileContents(RawFileContentsSource source)
        {
            object[] arguments = new object[]
            {
                new StringValue { Value = source.Path },
                new StringValue { Value = source.NameRegex.SearchRegex },
                new StringValue { Value = source.NameRegex.Replacement },
                new StringValue { Value = source.ReturnValueRegex.SearchRegex },
                new StringValue { Value = source.ReturnValueRegex.Replacement },
            };

            return ExecuteFunction("LoadRawFileContents", arguments);
        }

        public static IEnumerable<ExecutableItem> ExecuteFunction(AhkFunctionContentsSource contentSource)
        {
            return ExecuteFunction(contentSource.Function, contentSource.Arguments.ToArray());
        }

        private static IEnumerable<ExecutableItem> ExecuteFunction(
            string function, params object[] arguments)
        {
            using (var waitHandle = new ManualResetEvent(false))
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => waitHandle.Set();

                try
                {
                    MainForm.AhkFunctionResultReported += action;
                    ExecuteMethod(function, arguments.ToArray());
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
                executableItem.Arguments.Add(new StringValue
                {
                    Value = result[i + 1]
                });
                yield return executableItem;
            }
        }

        public static void ExecuteMethod(string name, params object[] arguments)
        {
            string[] properArguments = arguments.Select(x =>
            {
                var stringValue = x as StringValue;
                if (stringValue != null)
                {
                    return "\"" + stringValue.Value + "\"";
                }
                var ahkVariable = x as AhkVariable;
                if (ahkVariable != null)
                {
                    return ahkVariable.Value;
                }
                throw new Exception("Unknown argument type");
            }).ToArray();

            using (Process process = Process.GetProcessesByName("AutoHotKey").Single())
            {
                SerializeMethodInfo(name, properArguments);
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
