using System.Windows.Forms;

namespace Ahk
{
    public static class Extensions
    {
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
    }
}
