using System.Collections.Generic;
using AutomationEngine;

namespace AutomationExtensions
{
    public class SaveUrlCommand : IPluginExecutor
    {
        public string Id => "save_url";

        public void Execute(params string[] arguments)
        {
            List<string> result = AhkInterop.ExecuteFunction("Chrome_GetUrl");
        }
    }
}
