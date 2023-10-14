using System;

namespace BTurk.Automation.Core.Views;

public interface IAsyncExecutionDialog
{
    void Start(Action action);
}