using System;
using System.Collections.Generic;
using System.Linq;

namespace Ragnar.Mock.UnitTesting.Dispatcher
{
    public static class UnitTestHelper
    {
        public static Mock.Dispatcher.CommandResponse CreateCommandReponse(Type commandType, bool isAuthorized = false, List<Mock.Dispatcher.Validation.ValidationError> errors = null)
        {
            return new Mock.Dispatcher.CommandResponse()
            {
                IsAuthorized = isAuthorized,
                Errors = errors
            };
        }

        public static T AddContent<T>(this Mock.Dispatcher.CommandResponse commandReponse, T content)
        {
            commandReponse.Content = content;

            return content;
        }

        public static Mock.Dispatcher.Validation.ValidationError AddValidationError(this Mock.Dispatcher.CommandResponse commandReponse, string field = null, string message = null)
        {
            Mock.Dispatcher.Validation.ValidationError validationError = new Mock.Dispatcher.Validation.ValidationError()
            {
                Field = field,
                Message = message
            };

            commandReponse.Errors = (commandReponse.Errors ?? new List<Mock.Dispatcher.Validation.ValidationError>()).Concat(new[] { validationError }).ToList();

            return validationError;
        }
    }
}
