using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AutomationEngine
{
    public static class Extensions
    {
        [DllImport("user32.dll")]
        private static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        [DllImport("User32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        public static bool ContainsPartially(this string testedText, string value)
        {
            return ContainsPartially(testedText, value, false);
        }

        public static bool StartsPartiallyWith(this string testedText, string value)
        {
            return ContainsPartially(testedText, value, true);
        }

        private static bool ContainsPartially(string testedText, string value, bool mustMatchAtStart)
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

        private static string GetCharsFromKeys(uint key)
        {
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var buf = new StringBuilder(256);

            ToUnicode(key, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

        public static char ToCharacter(this Keys key)
        {
            string letter = GetCharsFromKeys((uint)key);
            if (letter.Length == 1)
            {
                return letter[0];
            }

            return (char)((int)key);
        }

        public static IEnumerable<Menu> GetParentMenus(this BaseItem item)
        {
            Menu parent = item.ParentMenu;
            while (parent != null)
            {
                yield return parent;
                parent = parent.ParentMenu;
            }
        }

        public static IEnumerable<BaseItem> ChildItems(this BaseItem item)
        {
            yield return item;

            Menu menu = item as Menu;
            if (menu == null)
            {
                yield break;
            }

            foreach (BaseItem childItem in menu.Items.SelectMany(x => x.ChildItems()))
            {
                yield return childItem;
            }
        }

        public static ActionType GetProperActionType(this ExecutableItem executableItem)
        {
            if (executableItem.ActionType != ActionType.None)
            {
                return executableItem.ActionType;
            }

            foreach (Menu parentMenu in executableItem.GetParentMenus())
            {
                if (parentMenu.ActionType != ActionType.None)
                {
                    return parentMenu.ActionType;
                }
            }

            return ActionType.None;
        }

        public static void LoadExecutionTimeStamps(this Menu menu)
        {
            menu.Items.ForEach(SetExecutionTimeStamp);
        }

        private static void SetExecutionTimeStamp(BaseItem item)
        {
            item.LastAccess = ExecutionTimeStamps.Instance.GetTimeStamp(item.Id);
            var menu = item as Menu;
            menu?.LoadExecutionTimeStamps();
        }

        public static TimeSpan FromTimeSpanString(this string value)
        {
            TimeSpan timeSpan;
            TimeSpan.TryParseExact(value, @"h\:mm", CultureInfo.InvariantCulture, out timeSpan);
            return timeSpan;
        }

        public static string ToTimeSpanString(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"h\:mm");
        }

        public static string GetExecutingMethodName(this BaseItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.ExecutingMethodName))
            {
                return item.ExecutingMethodName;
            }
            foreach (Menu menu in item.GetParentMenus())
            {
                if (menu.ExecutingMethodName != null)
                {
                    return menu.ExecutingMethodName;
                }
            }
            return null;
        }

        public static DateTime GetPreviousMonthStart(this DateTime dateTime)
        {
            var temp = dateTime.AddMonths(-1);
            return new DateTime(temp.Year, temp.Month, 1);
        }
    }
}
