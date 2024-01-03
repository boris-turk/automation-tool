using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Requests;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

public class CommandProcessor : ICommandProcessor
{
    public CommandProcessor(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

    void ICommandProcessor.Process(ICommand command)
    {
        GenericMethodInvoker.Instance(this)
            .Method(nameof(Handle))
            .WithGenericTypes(command.GetType())
            .WithArguments(command)
            .Invoke();
    }

    public void Handle<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = Container.GetInstance<ICommandHandler<TCommand>>();
        handler.Handle(command);
    }
}