namespace Ragnar.Mock.Dispatcher.Authorization
{
    public class CommandAuthorizerBase<TCommand> : ICommandAuthorizer<TCommand>
        where TCommand : ICommand
    {
        private readonly IAuthorizer authorizer;

        public CommandAuthorizerBase(IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
        }

        public virtual bool Authorize(TCommand command)
        {
            return authorizer.CheckAccess();
        }
    }
}