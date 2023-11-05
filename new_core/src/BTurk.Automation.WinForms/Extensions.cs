using System.Collections.Generic;
using System.Windows.Forms;

namespace BTurk.Automation.WinForms;

public static class Extensions
{
    public static IEnumerable<T> GetAllChildControls<T>(this Control control)
    {
        var allChildControls = new List<T>();

        foreach (Control childControl in control.Controls)
        {
            if (childControl is T specificControl)
                allChildControls.Add(specificControl);

            if (childControl.HasChildren)
                allChildControls.AddRange(GetAllChildControls<T>(childControl));
        }

        return allChildControls;
    }
}