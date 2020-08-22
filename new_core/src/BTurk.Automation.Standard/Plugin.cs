using BTurk.Automation.Core;
using BTurk.Automation.Core.Plugins;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class Plugin : IPlugin
    {
        public void Setup()
        {
            var collection = Bootstrapper.GetInstance<ISearchHandlersCollection>();
            collection.AddHandler(new CommitSearchHandler());
        }

        public void Teardown()
        {
        }
    }
}
