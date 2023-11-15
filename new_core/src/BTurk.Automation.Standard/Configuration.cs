using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BTurk.Automation.Standard;

[DataContract]
public class Configuration
{
    [DataMember(Name = "ProgramPaths")]
    public List<ProgramPath> ProgramPaths { get; set; }

    [DataMember(Name = "Directories")]
    public List<DirectoryPath> Directories { get; set; }

    public string GetProgramPath(string programName)
    {
        var programPath = ProgramPaths.FirstOrDefault(p => p.Name == programName)?.Path;

        if (programPath == null)
            throw new InvalidOperationException($"Missing \"{programName}\" program path in configuration.");

        return programPath;
    }

    public string GetDirectoryPath(string directoryId)
    {
        var directoryPath = Directories.FirstOrDefault(p => p.Name == directoryId)?.Path;

        if (directoryPath == null)
            throw new InvalidOperationException($"Missing \"{directoryId}\" directory path in configuration.");

        return directoryPath;
    }
}