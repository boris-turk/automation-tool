namespace BTurk.Automation.Core
{
    public interface IPlugin
    {
        void Setup(string executingDirectory);
        void Teardown();
    }
}