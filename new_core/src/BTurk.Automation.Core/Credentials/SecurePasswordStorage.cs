using System;
using System.Runtime.InteropServices;
using System.Security;

namespace BTurk.Automation.Core.Credentials;

public class SecurePasswordStorage
{
    private static SecureString _storedPassword;

    public static void StorePassword(string password)
    {
        _storedPassword = new SecureString();

        foreach (var character in password)
            _storedPassword.AppendChar(character);

        _storedPassword.MakeReadOnly();
    }

    public static string RetrievePassword()
    {
        var ptr = IntPtr.Zero;

        try
        {
            ptr = Marshal.SecureStringToGlobalAllocUnicode(_storedPassword);
            return Marshal.PtrToStringUni(ptr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(ptr);
        }
    }
}
