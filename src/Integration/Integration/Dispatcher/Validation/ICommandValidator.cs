using System.Collections.Generic;

namespace Ragnar.Integration.Dispatcher.Validation
{
    public interface ICommandValidator<TCommand>
           where TCommand : ICommand
    {
        List<ValidationError> Validate(TCommand command);
    }
}