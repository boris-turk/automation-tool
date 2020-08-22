using BTurk.Automation.Host;
using BTurk.Automation.Host.Plugins;
using BTurk.Automation.Host.SearchEngine;

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
