using System;

namespace Ragnar.Integration.Dispatcher
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly SimpleInjector.Container container;

        public CommandDispatcher(SimpleInjector.Container container)
        {
            this.container = container;
        }

        public ICommandResponse Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type commandType = command.GetType();

            CommandResponse commandResponse = new CommandResponse();

            Authorization.ICommandAuthorizer<TCommand> commandAuthorized = container.GetInstance(typeof(Authorization.CommandAuthorizerBase<>).MakeGenericType(commandType)) as Authorization.ICommandAuthorizer<TCommand>;

            commandResponse.IsAuthorized = commandAuthorized.Authorize(command);

            if (commandResponse.IsAuthorized)
            {
                Validation.ICommandValidator<TCommand> commandValidator = container.GetInstance(typeof(Validation.CommandValidatorBase<>).MakeGenericType(commandType)) as Validation.ICommandValidator<TCommand>;

                commandResponse.Errors = commandValidator.Validate(command);

                if (commandResponse.IsValid)
                {
                    ICommandHandler<TCommand> commandHandler = container.GetInstance(typeof(ICommandHandler<>).MakeGenericType(commandType)) as ICommandHandler<TCommand>;

                    commandResponse.Content = commandHandler.Execute(command);
                }
            }

            return commandResponse;
        }
    }
}
