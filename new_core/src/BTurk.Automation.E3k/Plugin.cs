using BTurk.Automation.Core;
using BTurk.Automation.Core.Plugins;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k
{
    public class Plugin : IPlugin
    {
        public void Setup()
        {
            var collection = Bootstrapper.GetInstance<ISearchHandlersCollection>();
            collection.AddHandler(new FieldSearchHandler());
        }

        public void Teardown()
        {
        }
    }
}