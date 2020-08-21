namespace BTurk.Automation.Core.Plugins
{
    public interface IPlugin
    {
        void Setup(string executingDirectory);
        void Teardown();
    }
}