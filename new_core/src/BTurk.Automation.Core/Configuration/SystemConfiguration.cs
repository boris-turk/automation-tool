using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace BTurk.Automation.Core.Configuration;

[DataContract]
public class SystemConfiguration
{
    [DataMember(Name = "Programs")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<FileItem> ProgramPaths { get; set; }

    [DataMember(Name = "Files")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<FileItem> FilePaths { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string GetProgramPath(string name)
    {
        var path = ProgramPaths.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Path;

        if (path != null)
            return path;

        throw new InvalidOperationException($"Missing \"{name}\" program path in configuration.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string GetFilePath(string name)
    {
        var path = FilePaths.FirstOrDefault(p => p.Name == name)?.Path;

        if (path != null)
            return path;

        throw new InvalidOperationException($"Missing \"{name}\" file path in configuration.");
    }
}