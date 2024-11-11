using System;
using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.WinApi;

namespace BTurk.Automation.Core.SearchEngine;

[IgnoreUnusedTypeWarning<WindowFocusHandler>]
public class WindowFocusHandler : IMessageHandler<ShowingAutomationWindowMessage>,
    IMessageHandler<BeforeExecuteCommandMessage>
{
    private IntPtr? _originalWindow;

    public WindowFocusHandler(ISearchEngine searchEngine)
    {
        SearchEngine = searchEngine;
    }

    private ISearchEngine SearchEngine { get; }

    public void Handle(ShowingAutomationWindowMessage message)
    {
        _originalWindow = null;

        if (message != ShowingAutomationWindowMessage.ApplicationMenu)
            return;

        _originalWindow = Methods.GetActiveWindow();
    }

    public void Handle(BeforeExecuteCommandMessage message)
    {
        if (_originalWindow == null)
        {
            SearchEngine.Hide();
            return;
        }

        var handle = _originalWindow.Value;

        Methods.SetActiveWindow(handle);

        if (!Methods.IsWindow(handle))
        {
            message.Canceled = true;
            return;
        }

        var response = Methods.SendMessageTimeout(handle, Constants.WM_NULL, IntPtr.Zero, IntPtr.Zero, 0, 5000, out _);

        if (response == IntPtr.Zero || Methods.GetActiveWindow() != _originalWindow)
        {
            message.Canceled = true;
        }

        if (!message.Canceled)
            SearchEngine.Hide();
    }
}