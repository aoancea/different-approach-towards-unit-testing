namespace Ragnar.Integration.Dispatcher.Authorization
{
    public interface ICommandAuthorizer<TCommand>
           where TCommand : ICommand
    {
        bool Authorize(TCommand command);
    }
}