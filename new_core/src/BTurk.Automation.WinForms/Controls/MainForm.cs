using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.WinForms.Controls;

public partial class MainForm : Form, ISearchEngine, ISearchEngineV2
{
    public static Font PreferredFont = new("Microsoft Sans Serif", 12F);

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

    public IRequestV2 RootMenuRequest { get; set; }

    IRequestV2 ISearchEngineV2.RootMenuRequest => RootMenuRequest;

    public IRequestActionDispatcherV2 Dispatcher { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

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

    public ListBox ListBox => _listBox;

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

        if (args.KeyCode == Keys.Down)
        {
            SelectItem(ListBox.SelectedIndex + 1);
            handled = true;
        }

        if (args.KeyCode == Keys.Up)
        {
            SelectItem(ListBox.SelectedIndex - 1);
            handled = true;
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

    public string SearchText
    {
        [DebuggerStepThrough] 
        get => TextBox.Text;

        [DebuggerStepThrough] 
        set => TextBox.Text = value;
    }

    public Request SelectedItem
    {
        [DebuggerStepThrough] 
        get => (Request)ListBox.SelectedItem;
    }

    public List<SearchToken> SearchTokens
    {
        get => GetSearchTokens();
        set { }
    }

    private List<SearchToken> GetSearchTokens()
    {
        var searchText = TextBox.Text;

        var words = searchText.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries).ToArray();
        var tokens = words.Select(w => new WordToken(w)).Cast<SearchToken>().ToList();

        if (tokens.Any() && (searchText.EndsWith(" ") || searchText.EndsWith("\t")))
            tokens.Insert(tokens.Count, new SpaceToken());

        return tokens;
    }

    public SearchResult SelectedSearchResult => (SearchResult)ListBox.SelectedItem;

    public void SetSearchResults(List<SearchResult> resultsCollection)
    {
        ListBox.BeginUpdate();

        ListBox.Items.Clear();

        foreach (var item in resultsCollection)
            ListBox.Items.Add(item);

        ListBox.EndUpdate();

        SelectItem(itemIndex: 0);
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x0312)
            OnGlobalShortcutKeyPressed(m.WParam.ToInt32());

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