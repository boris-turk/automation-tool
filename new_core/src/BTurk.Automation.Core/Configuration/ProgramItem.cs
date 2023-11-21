using System.Runtime.Serialization;

namespace BTurk.Automation.Core.Configuration;

[DataContract]
public class ProgramItem
{
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Path")]
    public string Path { get; set; }
}