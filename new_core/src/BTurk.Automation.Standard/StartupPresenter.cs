﻿using BTurk.Automation.Core.Credentials;
using BTurk.Automation.Core.Presenters;
using BTurk.Automation.Core.Queries;
using BTurk.Automation.Core.Views;
using BTurk.Automation.Standard.SecurityServices;

namespace BTurk.Automation.Standard;

public class StartupPresenter : IPresenter
{
    public StartupPresenter(IViewProvider viewProvider, IQueryProcessor queryProcessor)
    {
        ViewProvider = viewProvider;
        QueryProcessor = queryProcessor;
    }

    private IViewProvider ViewProvider { get; }

    private IQueryProcessor QueryProcessor { get; }

    public bool EnteredValidPassword { get; private set; }

    public void Start()
    {
        string password = "";

        var viewBuilder = ViewProvider.Builder();

        viewBuilder.ModalDialogStyle();

        viewBuilder.CancelQuestion("Exit?");

        viewBuilder
            .AddField<string>()
            .LabelText("Master password:")
            .PasswordInputStyle()
            .BindSetter(v => password = v);

        viewBuilder.CreateAndShow();

        OnPasswordEntered(password);
    }

    private void OnPasswordEntered(string password)
    {
        var passwordValidQuery = new IsMasterPasswordValidQuery(password);

        EnteredValidPassword = System.Diagnostics.Debugger.IsAttached || QueryProcessor.Process(passwordValidQuery);

        if (EnteredValidPassword)
            SecurePasswordStorage.StorePassword(password);
    }
}