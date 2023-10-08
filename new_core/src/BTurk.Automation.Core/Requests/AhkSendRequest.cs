using System.Runtime.Serialization;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.Requests;

[DataContract]
public class AhkSendRequest : Request, ICommand
{
    public AhkSendRequest()
    {
        Command = this;
    }

    public string Keys { get; set; }
}