using System.Runtime.Serialization;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.Requests;

[DataContract]
public class OpenProgramRequest : Request, ICommand
{
    public OpenProgramRequest(string programName)
        : base(programName)
    {
        Command = this;
        ProgramName = programName;
    }

    public string ProgramName { get; }
}