using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.E3k
{
    public class Module : Request
    {
        public Module(int number, string name)
            : base($"{number:000} - {name}")
        {
            Number = number;
            Name = name;
        }

        public int Number { get; }

        public string Name { get; }
    }
}