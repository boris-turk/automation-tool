using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.WinForms.Interop;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.WinForms.Controls;

public partial class MainForm : Form, ISearchEngine
{
    public static Font PreferredFont = new("Microsoft Sans Serif", 12F);

    public event Action BeforeDispose;

    private const int OutOfScreenOffset = -20000;

    public MainForm()
    {
        InitializeComponent();

        Font = PreferredFont;
        TopMost = false;
        KeyPreview = true;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        Location = new Point(OutOfScreenOffset, OutOfScreenOffset);

        Closing += (_, args) =>
        {
            args.Cancel = true;
            Visible = false;
        };

        _listBox.SelectedIndexChanged += (_, _) => MoveFocusToTextBox();

        TextBox.KeyDown += (_, args) => OnTextBoxKeyDown(args);
    }

    public IRequest RootMenuRequest { get; set; }

    IRequest ISearchEngine.RootMenuRequest => RootMenuRequest;

    public IRequestActionDispatcher Dispatcher { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    public EnvironmentContext Context => EnvironmentContextProvider.Context;

    public IEnvironmentContextProvider EnvironmentContextProvider { get; set; }

    public IMessagePublisher MessagePublisher { get; set; }

    private void MoveFocusToTextBox()
    {
        if (!TextBox.Focused)
        {
            TextBox.Focus();
        }
    }

    public TextBox TextBox { get; private set; }

    public CustomListBox ListBox => _listBox;

    public bool WorkInProgressVisible
    {
        get => _workInProgressPictureBox.Visible;
        set
        {
            _workInProgressPictureBox.Visible = value;

            if (value)
                _workInProgressPictureBox.BringToFront();
            else
                _workInProgressPictureBox.SendToBack();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        var handled = ProcessKeyDown(e);

        if (handled)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        base.OnKeyDown(e);
    }

    private bool ProcessKeyDown(KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Escape:
                Hide();
                return true;

            case Keys.Enter when !e.Alt && !e.Control && !e.Shift:
                Dispatcher.Dispatch(ActionType.Execute);
                return true;

            default:
                return false;
        }
    }

    private void OnTextBoxKeyDown(KeyEventArgs args)
    {
        var handled = false;

        if (args.KeyCode is Keys.Up or Keys.Down or Keys.PageUp or Keys.PageDown)
        {
            ListBox.OnNavigationKeyPressed(args.KeyCode);
        }

        if (args.KeyData == (Keys.Control | Keys.Back))
        {
            SendKeys.SendWait("^+{LEFT}{BACKSPACE}");
            handled = true;
        }

        if (handled)
        {
            args.SuppressKeyPress = true;
            args.Handled = true;
        }
    }

    protected override void OnShown(EventArgs e)
    {
        if (Debugger.IsAttached)
            CenterToScreen();
        else
            Visible = false;

        base.OnShown(e);
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        if (Visible)
            Dispatcher.Dispatch(ActionType.Search);
        else if (TextBox.Text != "")
            TextBox.Text = "";
        else
            OnSearchTextChanged();
    }

    protected override void OnLoad(EventArgs e)
    {
        TextBox.TextChanged += (_, _) => OnSearchTextChanged();
        MessagePublisher.Publish(ShowingAutomationWindowMessage.MainMenu);
        base.OnLoad(e);
    }

    private void OnSearchTextChanged()
    {
        if (Visible)
            Dispatcher.Dispatch(ActionType.Search);
    }

    private void SelectItem(int itemIndex)
    {
        if (itemIndex < 0)
            return;

        if (ListBox.Items.Count > itemIndex)
            ListBox.SelectedIndex = itemIndex;
    }

    public void ToggleVisibility()
    {
        Visible = !Visible;

        if (!Visible)
            return;

        if (Location.X == OutOfScreenOffset)
            CenterToScreen();

        TopMost = true;
        Activate();
    }

    public List<SearchToken> SearchTokens
    {
        get => SearchToken.GetSearchTokens(TextBox.Text);
        set { }
    }

    public SearchResult SelectedSearchResult => (SearchResult)ListBox.SelectedItem;

    public void SetSearchResults(List<SearchResult> resultsCollection)
    {
        ListBox.BeginUpdate();

        ListBox.Items.Clear();

        bool separatorAdded = false;

        for (int i = 0; i < resultsCollection.Count; i++)
        {
            ListBox.Items.Add(resultsCollection[i]);

            if (!separatorAdded && ShouldAddSeparator(i, resultsCollection))
            {
                ListBox.AddSeparator();
                separatorAdded = true;
            }
        }

        ListBox.EndUpdate();

        SelectItem(itemIndex: 0);
    }

    private bool ShouldAddSeparator(int resultIndex, List<SearchResult> resultsCollection)
    {
        if (resultIndex == resultsCollection.Count - 1)
            return false;

        for (int i = 0; i < resultsCollection[resultIndex + 1].Items.Count - 1; i++)
        {
            var item1 = resultsCollection[resultIndex].Items.ElementAtOrDefault(i);
            var item2 = resultsCollection[resultIndex + 1].Items.ElementAtOrDefault(i);

            if (item1 != item2)
                return true;
        }

        return false;
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x0312)
            OnGlobalShortcutKeyPressed(m.WParam.ToInt32());

        if (m.Msg == Win32Constants.WM_DESTROY)
            BeforeDispose?.Invoke();

        base.WndProc(ref m);
    }

    private void OnGlobalShortcutKeyPressed(int shortcutId)
    {
        var message = shortcutId == GlobalShortcuts.OpenMainWindowShortcutId
            ? ShowingAutomationWindowMessage.MainMenu
            : ShowingAutomationWindowMessage.ApplicationMenu;

        MessagePublisher.Publish(message);

        ToggleVisibility();
    }
}