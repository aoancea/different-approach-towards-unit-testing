namespace Ragnar.Integration.Dispatcher
{
    public interface ICommandDispatcher
    {
        ICommandResponse Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    }
}