using System;

namespace BTurk.Automation.Core.AsyncServices
{
    public class ProgressData
    {
        public int Percent { get; }

        public int Total { get; }

        public int Current { get; }

        public string Text { get; }

        public object[] TextArguments { get; }

        public ProgressData(int percent)
        {
            Percent = percent;
        }

        public ProgressData(int current, int total)
        {
            Total = total;
            Current = Math.Min(current, total);
        }

        public ProgressData(string text)
        {
            Text = text ?? "";
        }

        public ProgressData(object[] textArguments)
        {
            TextArguments = textArguments;
        }
    }
}