using System.Runtime.Serialization;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

[DataContract]
public class UrlRequest : Request, IFileRequest
{
    public UrlRequest()
    {
        Command = new OpenWithDefaultProgramCommand(this);
    }

    [DataMember(Name = "Url")]
    public string Url { get; set; }

    string IFileRequest.Path => Url;
}