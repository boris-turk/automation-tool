namespace AutomationEngine.Messages
{
    public static class WindowMessages
    {
        public const int WmSyscommand = 0x0112;
        public const int ScMinimize = 0xf020;
        public const int ScMaximize = 0xf030;

        public const int WmCopydata = 0x004A;
        public const string AhkProcessId = "P";
        public const string ToggleGLobalMenuVisibility = "T";
        public const string ToggleContextMenuVisibility = "C";
        public const string AhkFunctionResultReported = "R";
    }
}
