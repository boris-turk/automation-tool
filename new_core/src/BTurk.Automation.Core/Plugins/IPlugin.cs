using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Plugins
{
    public interface IPlugin
    {
        void Setup(ISearchHandlersCollection collection);
        void Teardown();
    }
}