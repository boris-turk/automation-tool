using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.DependencyResolution
{
    public class CommandProcessor : ICommandProcessor
    {
        void ICommandProcessor.Process(ICommand command)
        {
            GenericMethodInvoker.Instance(this)
                .Method(nameof(Visit))
                .WithGenericTypes(command.GetType())
                .WithArguments(command)
                .Invoke();
        }

        public void Visit<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = Container.GetInstance<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }
    }
}