namespace BTurk.Automation.Core.FileSystem;

public class DirectoryParameters
{
    private DirectoryParameters(string id, object argument = null)
    {
        Id = id;
        Argument = argument;
    }

    public string Id { get; }

    public object Argument { get; }

    public static DirectoryParameters None { get; } = new(nameof(None));

    public static DirectoryParameters Configuration { get; } = new(nameof(Configuration));

    public static DirectoryParameters LocalConfiguration { get; } = new(nameof(LocalConfiguration));
}