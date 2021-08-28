using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenWithDefaultProgramCommand : ICommand
    {
        public OpenWithDefaultProgramCommand(IFileRequest fileRequest)
        {
            FileRequest = fileRequest;
        }

        public IFileRequest FileRequest { get; set; }
    }
}