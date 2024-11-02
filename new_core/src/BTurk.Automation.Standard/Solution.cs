using System.Runtime.Serialization;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

[DataContract]
public class Solution : Request, IFileRequest
{
    public Solution()
    {
        Configure()
            .SetCommand(new OpenWithDefaultProgramCommand(this));
    }

    [DataMember(Name = "Path")]
    public string Path { get; set; }
}