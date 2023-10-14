using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTurk.Automation.Core.AsyncServices;
using BTurk.Automation.Core.Views;

// ReSharper disable LocalizableElement

namespace BTurk.Automation.WinForms;

public class AsyncExecutionDialog : Form, IAsyncExecution, IAsyncExecutionDialog
{
    private const int SpaceBetweenLabelAndCancelButton = 30;

    public AsyncExecutionDialog(MainForm mainForm)
    {
        MainForm = mainForm;
        Size = new Size(200, 100);
        Padding = new Padding(10, 10, 10, 10);

        MaximizeBox = false;
        MinimizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;

        _messageLabel = new Label
        {
            AutoSize = true,
            Location = new Point(Padding.Left, Padding.Top)
        };

        _cancelButton = new Button { Text = "Cancel" };
        _cancelButton.Click += (_, _) => OnCancelRequested();

        CancelButton = _cancelButton;

        Controls.Add(_messageLabel);
        Controls.Add(_cancelButton);

        Load += OnDialogLoaded;
    }

    private Action _action;

    private readonly Label _messageLabel;

    private readonly Button _cancelButton;

    public MainForm MainForm { get; }

    public bool IsCanceled { get; private set; }

    public void SetProgressData(ProgressData data)
    {
        _messageLabel.Invoke((Action)(() => RefreshProgressData(data)));
    }

    private void RefreshProgressData(ProgressData data)
    {
        _messageLabel.Text = data.Text;
    }

    protected override void OnLoad(EventArgs e)
    {
        Text = "Executing ...";
        base.OnLoad(e);
    }

    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);

        if (_messageLabel == null)
            return;

        var contentWidth = _messageLabel.Width;
        var contentHeight = _messageLabel.Height + SpaceBetweenLabelAndCancelButton + _cancelButton.Height;

        if (contentWidth < 300)
            contentWidth = 300;

        if (contentHeight < 100)
            contentHeight = 100;

        Width = contentWidth + Padding.Left + Padding.Right;
        Height = contentHeight + Padding.Top + Padding.Bottom;

        _cancelButton.Location = new Point
        {
            X = ClientSize.Width / 2 - _cancelButton.Width / 2,
            Y = ClientSize.Height - Padding.Bottom - _cancelButton.Height
        };
    }

    private void OnCancelRequested()
    {
        IsCanceled = true;
    }

    private async void OnDialogLoaded(object sender, EventArgs e)
    {
        await Task.Run(_action);
        Close();
    }

    public void Start(Action action)
    {
        _action = action;
        ShowDialog(MainForm);
    }
}