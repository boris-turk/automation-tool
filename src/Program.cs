using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ahk
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //RunAhkScripts();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void RunAhkScripts()
        {

            //create an autohtkey engine.
            var ahk = new AutoHotkey.Interop.AutoHotkeyEngine();

            //Load a library or exec scripts in a file
            ahk.Load(@"C:\Users\Boris\Dropbox\Automation\functions.ahk");

            //execute a specific function (found in functions.ahk), with 2 parameters
            ahk.ExecFunction("MyFunction", "Hello", "World");
        }
    }
}
