using AutoHotkey.Interop;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.Requests
{
    public class AhkSendCommandHandler : ICommandHandler<AhkSendRequest>
    {
        public void Handle(AhkSendRequest command)
        {
            AutoHotkeyEngine.Instance.ExecRaw($"Send {command.Keys}");
        }
    }
}