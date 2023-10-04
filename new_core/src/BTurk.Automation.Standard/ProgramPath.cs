using System.Runtime.Serialization;

namespace BTurk.Automation.Standard;

[DataContract]
public class ProgramPath
{
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Path")]
    public string Path { get; set; }
}