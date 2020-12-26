using System.ComponentModel;

namespace BTurk.Automation.Core.WinApi
{
    public class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public KeyboardState KeyboardState { get; }
        public LowLevelKeyboardInputEvent KeyboardData { get; }

        public GlobalKeyboardHookEventArgs(LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}