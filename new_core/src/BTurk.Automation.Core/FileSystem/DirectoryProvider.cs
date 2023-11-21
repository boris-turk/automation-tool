using System;

namespace BTurk.Automation.Core.FileSystem;

public class DirectoryProvider : IDirectoryProvider
{
    public string GetDirectory(DirectoryParameters parameters)
    {
        if (parameters.Id == nameof(DirectoryParameters.None))
            return null;

        if (parameters.Id == nameof(DirectoryParameters.Configuration))
            return "..\\configuration";

        throw new NotSupportedException($"Cannot map \"{parameters.Id}\" into appropriate directory.");
    }
}