using System;
using System.Collections.Generic;

namespace Ragnar.Integration.UnitTesting.Dispatcher
{
    public class FakePinkIocContainer : Integration.Dispatcher.IIocContainer
    {
        private readonly Dictionary<Type, object> registeredTypes;

        public FakePinkIocContainer()
        {
            Integration.Dispatcher.Authorization.IAuthorizer authorizedMock = new AuthorizerMock();

            registeredTypes = new Dictionary<Type, object>()
            {
                { typeof(Integration.Dispatcher.Authorization.IAuthorizer), authorizedMock },

                { typeof(Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateOrangeCommand>), new Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateOrangeCommand>(authorizedMock) },
                { typeof(Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleCommand>), new Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleCommand>(authorizedMock) },
                { typeof(Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleUnauthorizedCommand>), new Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleUnauthorizedCommand>(authorizedMock) },
                { typeof(Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleInvalidCommand>), new Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleInvalidCommand>(authorizedMock) },

                { typeof(Integration.Dispatcher.Validation.CommandValidatorBase<CreateOrangeCommand>), new Integration.Dispatcher.Validation.CommandValidatorBase<CreateOrangeCommand>() },
                { typeof(Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleCommand>), new Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleCommand>() },
                { typeof(Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleUnauthorizedCommand>), new Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleUnauthorizedCommand>() },
                { typeof(Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleInvalidCommand>), new Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleInvalidCommand>() },

                { typeof(Integration.Dispatcher.ICommandHandler<CreateOrangeCommand>), new CreateOrangeCommandHandler() },
                { typeof(Integration.Dispatcher.ICommandHandler<CreateAppleCommand>), new CreateAppleCommandHandler() },
                { typeof(Integration.Dispatcher.ICommandHandler<CreateAppleUnauthorizedCommand>), new CreateAppleUnauthorizedCommandHandler() },
                { typeof(Integration.Dispatcher.ICommandHandler<CreateAppleInvalidCommand>), new CreateAppleInvalidCommandHandler() },
            };
        }

        public object GetInstance(Type type)
        {
            return registeredTypes[type];
        }
    }
}
