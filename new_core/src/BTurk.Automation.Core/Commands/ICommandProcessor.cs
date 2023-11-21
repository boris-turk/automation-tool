namespace BTurk.Automation.Core.Commands;

public interface ICommandProcessor
{
    void Process(ICommand command);
}