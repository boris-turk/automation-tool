using AutoHotkey.Interop;

namespace BTurk.Automation.Core.Requests
{
    public class AhkSendRequestExecutor : IRequestExecutor<AhkSendRequest>
    {
        public void Execute(RequestExecutionContext<AhkSendRequest> context)
        {
            AutoHotkeyEngine.Instance.ExecRaw($"Send {context.Request.Keys}");
        }
    }
}