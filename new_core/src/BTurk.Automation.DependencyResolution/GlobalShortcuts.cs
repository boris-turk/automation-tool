using System;
using System.Globalization;
using System.IO;
using System.Threading;
using BTurk.Automation.Core.WinApi;

namespace BTurk.Automation.DependencyResolution;

public class GlobalShortcuts
{
    public const int OpenMainWindowShortcutId = 1;
    public const int OpenAppContextWindowShortcutId = 2;

    private readonly string _filePath = Path.Combine(Path.GetTempPath(), "new_automation_activity.txt");

    private Timer _timer;
    private MainForm _form;
    private bool _shortcutsInstalled;

    public void Install()
    {
        _form = Container.GetInstance<MainForm>();

        _timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        _timer.Change(0, Timeout.Infinite);
    }

    public void Uninstall()
    {
        if (!_shortcutsInstalled)
            return;

        _timer?.Dispose();
        _timer = null;

        _shortcutsInstalled = false;

        _form.Invoke((Action)UnRegisterHotKeys);
    }
        
    private void OnTimerElapsed(object state)
    {
        WriteActivity();

        if (!_shortcutsInstalled)
        {
            Thread.Sleep(1000);
            _form.Invoke((Action)RegisterHotKeys);
        }

        _timer.Change(500, Timeout.Infinite);
    }

    private void WriteActivity()
    {
        var dateTime = DateTime.Now;
        var text = dateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt", CultureInfo.InvariantCulture);

        try
        {
            File.WriteAllText(_filePath, text);
        }
        catch
        {
            // ignore exception
        }
    }

    private void RegisterHotKeys()
    {
        _shortcutsInstalled = Methods.RegisterHotKey(
            _form.Handle, OpenMainWindowShortcutId, Constants.MOD_ALT, Constants.VK_SPACE);

        _shortcutsInstalled = Methods.RegisterHotKey(
            _form.Handle, OpenAppContextWindowShortcutId, Constants.MOD_ALT, Constants.VK_OEM_1);
    }

    private void UnRegisterHotKeys()
    {
        Methods.UnregisterHotKey(_form.Handle, OpenMainWindowShortcutId);
        Methods.UnregisterHotKey(_form.Handle, OpenAppContextWindowShortcutId);
    }
}