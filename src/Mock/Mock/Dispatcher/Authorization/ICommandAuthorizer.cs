namespace Ragnar.Mock.Dispatcher.Authorization
{
    public interface ICommandAuthorizer<TCommand>
           where TCommand : ICommand
    {
        bool Authorize(TCommand command);
    }
}