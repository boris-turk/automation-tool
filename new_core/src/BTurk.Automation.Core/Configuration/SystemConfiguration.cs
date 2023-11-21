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
    public List<ProgramItem> ProgramPaths { get; set; }

    [DataMember(Name = "Directories")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<DirectoryItem> Directories { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string GetProgramPath(string programName)
    {
        var programPath = ProgramPaths.FirstOrDefault(p => p.Name == programName)?.Path;

        if (programPath != null)
            return programPath;

        throw new InvalidOperationException($"Missing \"{programName}\" program path in configuration.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string GetDirectoryPath(string directoryId)
    {
        var directoryPath = Directories.FirstOrDefault(p => p.Name == directoryId)?.Path;

        if (directoryPath != null)
            return directoryPath;

        throw new InvalidOperationException($"Missing \"{directoryId}\" directory path in configuration.");
    }
}