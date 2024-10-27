using System.Runtime.Serialization;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.Requests;

[DataContract]
public class OpenProgramRequest : Request, ICommand
{
    public OpenProgramRequest(string programName)
    {
        Command = this;
        ProgramName = programName;

        Configure()
            .SetText(programName)
            .SetCommand(this);
    }

    public string ProgramName { get; }
}