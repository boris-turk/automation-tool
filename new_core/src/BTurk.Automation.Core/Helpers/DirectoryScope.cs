using System;
using System.IO;
using System.Linq;

namespace BTurk.Automation.Core.Helpers;

public class DirectoryScope
{
    public DirectoryScope(string directory)
    {
        Directory = directory;
    }

    public string Directory { get; }

    public bool EndsWith(string text)
    {
        text = TrimTrailingDirectorySeparatorChar(text);
        return Directory.EndsWith(text, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsHiddenDirectory()
    {
        var text = $"{Path.DirectorySeparatorChar}.";
        return Directory.IndexOf(text, StringComparison.Ordinal) >= 0;
    }

    private string TrimTrailingDirectorySeparatorChar(string directory)
    {
        if (directory.Last() == Path.DirectorySeparatorChar)
            return directory.TrimEnd(Path.DirectorySeparatorChar);

        return directory;
    }
}