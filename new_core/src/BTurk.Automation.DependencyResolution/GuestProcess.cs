using System;
using System.Windows.Forms;
using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Host.AssemblyLoading;
using BTurk.Automation.Standard;
using BTurk.Automation.WinForms;
using BTurk.Automation.WinForms.Controls;

// ReSharper disable LocalizableElement

namespace BTurk.Automation.DependencyResolution;

[IgnoreUnusedTypeWarning<GuestProcess>]
public class GuestProcess : IGuestProcess
{
    private MainForm _mainForm;
    private GlobalShortcuts _globalShortcuts;

    public void Start()
    {
        try
        {
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

            Bootstrapper.InitializeContainer();

            if (!AskForCredentials())
                return;

            _globalShortcuts = Bootstrapper.Container.GetInstance<GlobalShortcuts>();
            _mainForm = Bootstrapper.Container.GetInstance<MainForm>();
            _mainForm.Load += (_, _) => _globalShortcuts.Install();

            Application.Run(_mainForm);
        }
        catch (Exception e)
        {
            ReportError(e);
        }
        finally
        {
            DisposeResources();
        }
    }

    private bool AskForCredentials()
    {
        var presenter = Bootstrapper.Container.GetInstance<StartupPresenter>();
        presenter.Start();
        return presenter.EnteredValidPassword;
    }

    public void Dispose()
    {
        if (_mainForm is { IsDisposed: false })
            _mainForm?.Invoke(_mainForm.Dispose);
    }

    private void DisposeResources()
    {
        if (_mainForm is { IsDisposed: false })
            _mainForm?.Dispose();
    }

    private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e?.ExceptionObject is Exception exception)
            ReportError(exception);
        else
            ReportError(null);
    }

    private static void ReportError(Exception exception)
    {
        var errorMessage = StartupProcess.GetErrorMessage(exception);
        StartupProcess.LogErrorMessage(errorMessage);
        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}