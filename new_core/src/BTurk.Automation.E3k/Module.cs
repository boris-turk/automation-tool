using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.E3k;

public class Module : Request
{
    public Module(int number, string name)
    {
        Number = number;
        Name = name;
        Configure().SetText($"{number:000} - {name}");
    }

    public int Number { get; }

    public string Name { get; }
}