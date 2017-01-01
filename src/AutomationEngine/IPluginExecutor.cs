namespace AutomationEngine
{
    public interface IPluginExecutor
    {
        string Id { get; }
        void Execute(params string[] arguments);
    }
}