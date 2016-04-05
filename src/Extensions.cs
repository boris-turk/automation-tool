using System;
using System.Windows.Forms;

namespace AutomationEngine
{
    public static class Extensions
    {
        public static bool PartiallyContains(this string testedText, string value)
        {
            return PartiallyContains(testedText, value, false);
        }

        public static bool StartsPartiallyWith(this string testedText, string value)
        {
            return PartiallyContains(testedText, value, true);
        }

        private static bool PartiallyContains(string testedText, string value, bool mustMatchAtStart)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            int j = 0;
            for (int i = 0; i < testedText.Length; i++)
            {
                if (j >= value.Length)
                {
                    break;
                }
                if (char.ToUpperInvariant(value[j]) == char.ToUpperInvariant(testedText[i]))
                {
                    j++;
                }
                else if (j == 0 && mustMatchAtStart)
                {
                    return false;
                }
            }

            return j == value.Length;
        }
        public static T InvokeQuery<T>(this Control c, Func<T> m)
        {
            if (c.InvokeRequired)
            {
                return (T) c.Invoke(m);
            }

            return m();
        }

        public static void InvokeCommand(this Control c, MethodInvoker m)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(m);
            }
            else
            {
                m();
            }
        }

        public static void AutoFillFrom(this Control target, Control source)
        {
            EventHandler action = (sender, args) =>
            {
                if (target.Text.Trim().Length == 0)
                {
                    target.Text = source.Text;
                }
            };
            target.GotFocus -= action;
            target.GotFocus += action;
        }
    }
}
