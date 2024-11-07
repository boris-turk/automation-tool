using System.Drawing;
using System.Windows.Forms;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace BTurk.Automation.WinForms.Controls;

public sealed class CustomListBox : ListBox
{
    private const int WM_LBUTTONDOWN = 0x0201;

    public CustomListBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
    }

    private bool IsSeparator(int index) => Items[index] is ListBoxSeparator;

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0)
        {
            return;
        }

        if ((e.State & DrawItemState.Selected) != DrawItemState.Selected || IsSeparator(e.Index))
        {
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
        }
        else
        {
            e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
        }

        if (IsSeparator(e.Index))
        {
            using var pen = new Pen(Color.LightGray, 1);

            var point1 = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height / 2);
            var point2 = new Point(e.Bounds.Right, e.Bounds.Top + e.Bounds.Height / 2);

            e.Graphics.DrawLine(pen, point1, point2);
        }
        else
        {
            e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
        }

        e.DrawFocusRectangle();
    }

    public void OnNavigationKeyPressed(Keys key)
    {
        var index = SelectedIndex;

        if (ModifierKeys == Keys.Control && key == Keys.PageUp)
        {
            index = 0;
            while (index < Items.Count && IsSeparator(index))
            {
                index++;
            }
            SelectedIndex = index < Items.Count ? index : SelectedIndex;
            return;
        }

        if (ModifierKeys == Keys.Control && key == Keys.PageDown)
        {
            index = Items.Count - 1;
            while (index >= 0 && IsSeparator(index))
            {
                index--;
            }
            SelectedIndex = index >= 0 ? index : SelectedIndex;
            return;
        }

        var itemsPerPage = ClientSize.Height / ItemHeight;
        var direction = key is Keys.Up or Keys.PageUp ? -1 : 1;
        var step = key is Keys.PageUp or Keys.PageDown ? itemsPerPage : 1;

        for (int i = 0; i < step; i++)
        {
            int nextIndex = index + direction;

            if (nextIndex < 0 && direction == -1 || nextIndex >= Items.Count && direction == 1)
            {
                break;
            }

            index = nextIndex;
            while (IsSeparator(index))
            {
                index += direction;

                if (index < 0 || index >= Items.Count)
                {
                    return;
                }
            }
        }

        SelectedIndex = index;
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_LBUTTONDOWN)
        {
            var mousePosition = PointToClient(Cursor.Position);
            var index = IndexFromPoint(mousePosition);

            if (index >= 0 && IsSeparator(index))
            {
                return;
            }
        }
        
        base.WndProc(ref m);
    }

    public void AddSeparator()
    {
        Items.Add(new ListBoxSeparator());
    }

    private class ListBoxSeparator;
}