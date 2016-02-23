using System;

namespace AutomationEngine
{
    [Serializable]
    public class RawFileContentsSource : ContentSource
    {
        public string Path { get; set; }
    }
}