using BTurk.Automation.Standard;

namespace BTurk.Automation.E3k;

public static class Extensions
{
    public static bool IsTrunk(this Repository repository) => repository.Text == "trunk";
    public static bool IsClean(this Repository repository) => repository.Text == "clean";

    public static int Revision(this Repository repository)
    {
        var text = repository.Text.TrimStart('r', 'R');
        int.TryParse(text, out var number);
        return number;
    }
}