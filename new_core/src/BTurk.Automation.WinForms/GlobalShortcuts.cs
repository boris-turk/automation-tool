using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using BTurk.Automation.Core.WinApi;
using BTurk.Automation.WinForms.Controls;

namespace BTurk.Automation.WinForms;

public class GlobalShortcuts
{
    private static readonly HashSet<int> RegisteredHotKeys = [];

    public const int OpenMainWindowShortcutId = 1;
    public const int OpenAppContextWindowShortcutId = 2;

    private readonly string _filePath = Path.Combine(Path.GetTempPath(), "new_automation_activity.txt");

    private Timer _timer;
    private readonly MainForm _form;
    private bool _uninstalled;
    private readonly object _lockKey = new();

    public GlobalShortcuts(MainForm mainForm)
    {
        _form = mainForm;
        _form.BeforeDispose += Uninstall;
    }

    public void Install()
    {
        _timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        _timer.Change(0, Timeout.Infinite);
    }

    private void Uninstall()
    {
        lock (_lockKey)
        {
            _timer?.Dispose();
            _timer = null;
        }

        UnRegisterHotKeys();

        _uninstalled = true;
    }
        
    private void OnTimerElapsed(object state)
    {
        WriteActivity();

        bool hotKeysNotYetRegistered;

        lock (_lockKey)
            hotKeysNotYetRegistered = !RegisteredHotKeys.Any();

        if (hotKeysNotYetRegistered)
        {
            Thread.Sleep(1000);

            try
            {
                _form.Invoke(RegisterHotKeys);
            }
            catch (Exception ex) when (ex is ObjectDisposedException or InvalidOperationException)
            {
                // this rare case occurs if an error crashes (disposing of) the main form before the parallel timer
                // registers hotkeys during new core startup
            }
        }

        lock (_lockKey)
            _timer?.Change(500, Timeout.Infinite);
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
        if (_uninstalled)
            return;

        lock (_lockKey)
        {
            if (RegisteredHotKeys.Any())
                return;
        }

        RegisterSingleHotKey(OpenMainWindowShortcutId, Constants.MOD_ALT, Constants.VK_SPACE);
        RegisterSingleHotKey(OpenAppContextWindowShortcutId, Constants.MOD_ALT, Constants.VK_OEM_1);
    }

    private void UnRegisterHotKeys()
    {
        lock (_lockKey)
        {
            foreach (int id in RegisteredHotKeys)
                UnRegisterSingleHotKey(id);
        }
    }

    private void RegisterSingleHotKey(int id, uint modifiers, uint virtualKey)
    {
        lock (_lockKey)
        {
            var shortcutInstalled = Methods.RegisterHotKey(_form.Handle, id, modifiers, virtualKey);

            if (shortcutInstalled)
                RegisteredHotKeys.Add(id);
        }
    }

    private void UnRegisterSingleHotKey(int id)
    {
        var result = Methods.UnregisterHotKey(_form.Handle, id);

        if (result)
            RegisteredHotKeys.Remove(id);
    }
}