using System.Runtime.Serialization;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.Requests;

[DataContract]
public class AhkSendRequest : Request, ICommand
{
    public AhkSendRequest()
    {
        Configure()
            .SetText(() => Text)
            .SetCommand(this);
    }

    public string Keys { get; set; }
}