namespace BTurk.Automation.Core
{
    public interface IProcessStarter
    {
        void Start(string fileName, string arguments = null);
    }
}