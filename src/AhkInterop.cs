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
        private const string ResultFromFunction = "ResultFromFunction";
        private const string ResultFromRawFile = "ResultFromRawFile";

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
            AbstractValue[] arguments = new AbstractValue[]
            {
                new StringValue { Value = source.Path },
                new StringValue { Value = source.NameRegex.SearchRegex },
                new StringValue { Value = source.NameRegex.Replacement },
                new StringValue { Value = source.ReturnValueRegex.SearchRegex },
                new StringValue { Value = source.ReturnValueRegex.Replacement },
            };

            return ExecuteFunction(ResultFromRawFile, "", arguments);
        }

        public static IEnumerable<ExecutableItem> ExecuteFunction(AhkFunctionContentsSource source)
        {
            List<AbstractValue> arguments = new List<AbstractValue>
            {
                new StringValue { Value = source.NameRegex.SearchRegex },
                new StringValue { Value = source.NameRegex.Replacement },
                new StringValue { Value = source.ReturnValueRegex.SearchRegex },
                new StringValue { Value = source.ReturnValueRegex.Replacement },
            };

            arguments.AddRange(source.Arguments);

            return ExecuteFunction(ResultFromFunction, source.Function, arguments.ToArray());
        }

        private static IEnumerable<ExecutableItem> ExecuteFunction(
            string returnType, string function, params AbstractValue[] arguments)
        {
            using (var waitHandle = new ManualResetEvent(false))
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => waitHandle.Set();

                try
                {
                    MainForm.AhkFunctionResultReported += action;
                    ExecuteMethod(returnType, function, arguments.ToArray());
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
