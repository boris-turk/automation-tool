using System;

namespace BTurk.Automation.DependencyResolution.AsyncServices
{
    public interface IAsyncExecutionDialog
    {
        void Start(Action action);
    }
}