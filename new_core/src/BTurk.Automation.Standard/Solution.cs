using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class Solution : Request
    {
        public Solution(string text) : base(text)
        {
        }

        public string AbsolutePath { get; set; }
    }
}