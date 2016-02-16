using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        public static void ExecFunction(string name, params string[] arguments)
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
