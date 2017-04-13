﻿using System.Collections.Generic;

namespace Ragnar.Mock.Dispatcher.Validation
{
    public class CommandValidatorBase<TCommand> : ICommandValidator<TCommand>
        where TCommand : ICommand
    {
        public virtual List<ValidationError> Validate(TCommand command)
        {
            return new List<ValidationError>();
        }
    }
}