using System.Diagnostics;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard
{
    public static class Extensions
    {
        public static void Open(this IFileRequest request)
        {
            Process.Start(request.Path);
        }
    }
}