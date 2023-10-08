using BTurk.Automation.Core.Commands;

// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests;

public interface ICommandProcessor
{
    void Process(ICommand command);
}