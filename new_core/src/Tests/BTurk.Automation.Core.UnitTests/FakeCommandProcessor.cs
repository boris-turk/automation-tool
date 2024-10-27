using System.Collections.Generic;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Core.UnitTests;

public class FakeCommandProcessor : ICommandProcessor
{
    public List<ICommand> ProcessedCommands { get; } = [];

    void ICommandProcessor.Process(ICommand command)
    {
        ProcessedCommands.Add(command);
    }
}