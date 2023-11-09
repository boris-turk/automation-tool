using System.Runtime.Serialization;

namespace BTurk.Automation.Standard;

[DataContract]
public class DirectoryPath
{
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Path")]
    public string Path { get; set; }
}