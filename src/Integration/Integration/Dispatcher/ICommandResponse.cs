using System;
using System.Collections.Generic;

namespace Ragnar.Integration.Dispatcher
{
    public interface ICommandResponse
    {
        bool IsAuthorized { get; set; }

        List<Validation.ValidationError> Errors { get; set; }

        bool IsValid { get; }

        object Content { get; set; }
    }
}