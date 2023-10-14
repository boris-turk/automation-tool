using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Views;

namespace BTurk.Automation.DependencyResolution.AsyncServices;

public class AsyncCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : IAsyncCommand
{
    public AsyncCommandHandlerDecorator(ICommandHandler<TCommand> decoratee, IAsyncExecutionDialog asyncDialog)
    {
        Decoratee = decoratee;
        AsyncDialog = asyncDialog;
    }

    public ICommandHandler<TCommand> Decoratee { get; }

    public IAsyncExecutionDialog AsyncDialog { get; }

    public void Handle(TCommand command)
    {
        AsyncDialog.Start(() => Decoratee.Handle(command));
    }
}