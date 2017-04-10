using System;
using System.Collections.Generic;
using System.Linq;

namespace Ragnar.Integration.UnitTesting.Dispatcher
{
    public static class UnitTestHelper
    {
        public static Integration.Dispatcher.CommandResponse CreateCommandReponse(Type commandType, bool isAuthorized = false, List<Integration.Dispatcher.Validation.ValidationError> errors = null)
        {
            return new Integration.Dispatcher.CommandResponse()
            {
                IsAuthorized = isAuthorized,
                Errors = errors
            };
        }

        public static T AddContent<T>(this Integration.Dispatcher.CommandResponse commandReponse, T content)
        {
            commandReponse.Content = content;

            return content;
        }

        public static Integration.Dispatcher.Validation.ValidationError AddValidationError(this Integration.Dispatcher.CommandResponse commandReponse, string field = null, string message = null)
        {
            Integration.Dispatcher.Validation.ValidationError validationError = new Integration.Dispatcher.Validation.ValidationError()
            {
                Field = field,
                Message = message
            };

            commandReponse.Errors = (commandReponse.Errors ?? new List<Integration.Dispatcher.Validation.ValidationError>()).Concat(new[] { validationError }).ToList();

            return validationError;
        }
    }
}
