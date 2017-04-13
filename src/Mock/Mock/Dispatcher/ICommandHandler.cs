namespace Ragnar.Mock.Dispatcher
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        object Execute(TCommand command);
    }
}