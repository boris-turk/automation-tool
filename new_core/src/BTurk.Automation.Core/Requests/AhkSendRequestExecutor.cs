using AutoHotkey.Interop;

namespace BTurk.Automation.Core.Requests
{
    public class AhkSendRequestExecutor : IRequestExecutor<AhkSendRequest>
    {
        public void Execute(AhkSendRequest request)
        {
            AutoHotkeyEngine.Instance.ExecRaw($"Send {request.Keys}");
        }
    }
}