using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Ragnar.Integration.UnitTesting.Dispatcher
{
    [TestClass]
    public class CommandDispatcherTest
    {
        private Integration.Dispatcher.CommandDispatcher commandDispatcher;

        [TestInitialize]
        public void TestInitialize()
        {
            SimpleInjector.Container container = new SimpleInjector.Container();

            Bootstrapper.Initialize(container);

            commandDispatcher = container.GetInstance<Integration.Dispatcher.CommandDispatcher>();
        }

        #region Default handlers

        [TestMethod]
        public void CommandDispatcher_Dispatch_DefaultHandlers_ReturnReponse()
        {
            Integration.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateOrangeCommand), isAuthorized: true, errors: new List<Integration.Dispatcher.Validation.ValidationError>());

            expectedCommandResponse.AddContent(new CreateOrangeCommandResponse() { Name = "Create orange command response!" });

            CreateOrangeCommand createOrangeCommand = new CreateOrangeCommand();

            Integration.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createOrangeCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }

        #endregion

        #region Custom handlers

        [TestMethod]
        public void CommandDispatcher_Dispatch_CustomHandlers_Unauthorized_ReturnReponse()
        {
            Integration.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateAppleUnauthorizedCommand));

            CreateAppleUnauthorizedCommand createAppleUnauthorizedCommand = new CreateAppleUnauthorizedCommand();

            Integration.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createAppleUnauthorizedCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }

        [TestMethod]
        public void CommandDispatcher_Dispatch_CustomHandlers_Invalid_ReturnReponse()
        {
            Integration.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateAppleInvalidCommand), isAuthorized: true);

            expectedCommandResponse.AddValidationError(field: "field", message: "message");

            CreateAppleInvalidCommand createAppleInvalidCommand = new CreateAppleInvalidCommand();

            Integration.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createAppleInvalidCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }

        [TestMethod]
        public void CommandDispatcher_Dispatch_CustomHandlers_ReturnReponse()
        {
            Integration.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateAppleCommand), isAuthorized: true, errors: new List<Integration.Dispatcher.Validation.ValidationError>());

            expectedCommandResponse.AddContent(new CreateOrangeCommandResponse() { Name = "Create apple command response!" });

            CreateAppleCommand createAppleCommand = new CreateAppleCommand();

            Integration.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createAppleCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }

        #endregion
    }

    public class CreateOrangeCommand : Integration.Dispatcher.ICommand
    {
    }

    public class CreateAppleCommand : Integration.Dispatcher.ICommand
    {
    }

    public class CreateAppleUnauthorizedCommand : Integration.Dispatcher.ICommand
    {
    }

    public class CreateAppleInvalidCommand : Integration.Dispatcher.ICommand
    {
    }

    public class CreateAppleCommandAuthorizer : Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleCommand>
    {
        public CreateAppleCommandAuthorizer(Integration.Dispatcher.Authorization.IAuthorizer authorizer)
            : base(authorizer)
        { }

        public override bool Authorize(CreateAppleCommand command)
        {
            return true;
        }
    }

    public class CreateAppleUnauthorizedCommandAuthorizer : Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleUnauthorizedCommand>
    {
        public CreateAppleUnauthorizedCommandAuthorizer(Integration.Dispatcher.Authorization.IAuthorizer authorizer)
            : base(authorizer)
        { }

        public override bool Authorize(CreateAppleUnauthorizedCommand command)
        {
            return false;
        }
    }

    public class CreateAppleInvalidCommandAuthorizer : Integration.Dispatcher.Authorization.CommandAuthorizerBase<CreateAppleInvalidCommand>
    {
        public CreateAppleInvalidCommandAuthorizer(Integration.Dispatcher.Authorization.IAuthorizer authorizer)
            : base(authorizer)
        { }

        public override bool Authorize(CreateAppleInvalidCommand command)
        {
            return true;
        }
    }

    public class CreateAppleCommandValidator : Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleCommand>
    {
        public CreateAppleCommandValidator()
        { }

        public override List<Integration.Dispatcher.Validation.ValidationError> Validate(CreateAppleCommand command)
        {
            return new List<Integration.Dispatcher.Validation.ValidationError>();
        }
    }

    public class CreateAppleUnauthorizedCommandValidator : Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleUnauthorizedCommand>
    {
        public CreateAppleUnauthorizedCommandValidator()
        { }

        public override List<Integration.Dispatcher.Validation.ValidationError> Validate(CreateAppleUnauthorizedCommand command)
        {
            return new List<Integration.Dispatcher.Validation.ValidationError>();
        }
    }

    public class CreateAppleInvalidCommandValidator : Integration.Dispatcher.Validation.CommandValidatorBase<CreateAppleInvalidCommand>
    {
        public CreateAppleInvalidCommandValidator()
        { }

        public override List<Integration.Dispatcher.Validation.ValidationError> Validate(CreateAppleInvalidCommand command)
        {
            return new List<Integration.Dispatcher.Validation.ValidationError>() { new Integration.Dispatcher.Validation.ValidationError() { Field = "field", Message = "message" } };
        }
    }

    public class CreateOrangeCommandHandler : Integration.Dispatcher.ICommandHandler<CreateOrangeCommand>
    {
        public object Execute(CreateOrangeCommand command)
        {
            return new CreateOrangeCommandResponse() { Name = "Create orange command response!" };
        }
    }

    public class CreateAppleCommandHandler : Integration.Dispatcher.ICommandHandler<CreateAppleCommand>
    {
        public object Execute(CreateAppleCommand command)
        {
            return new CreateOrangeCommandResponse() { Name = "Create apple command response!" };
        }
    }

    public class CreateAppleUnauthorizedCommandHandler : Integration.Dispatcher.ICommandHandler<CreateAppleUnauthorizedCommand>
    {
        public object Execute(CreateAppleUnauthorizedCommand command)
        {
            return new CreateAppleUnauthorizedCommandResponse() { Name = "Create apple unauthorized command response!" };
        }
    }

    public class CreateAppleInvalidCommandHandler : Integration.Dispatcher.ICommandHandler<CreateAppleInvalidCommand>
    {
        public object Execute(CreateAppleInvalidCommand command)
        {
            return new CreateAppleInvalidCommandResponse() { Name = "Create apple invalid command response!" };
        }
    }

    public class CreateOrangeCommandResponse
    {
        public string Name { get; set; }
    }

    public class CreateAppleCommandResponse
    {
        public string Name { get; set; }
    }

    public class CreateAppleUnauthorizedCommandResponse
    {
        public string Name { get; set; }
    }

    public class CreateAppleInvalidCommandResponse
    {
        public string Name { get; set; }
    }
}
