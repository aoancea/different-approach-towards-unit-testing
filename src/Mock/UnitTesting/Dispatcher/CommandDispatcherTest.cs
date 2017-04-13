using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Ragnar.Mock.UnitTesting.Dispatcher
{
    [TestClass]
    public class CommandDispatcherTest
    {
        private readonly Type commandAuthorizerType = typeof(Mock.Dispatcher.Authorization.CommandAuthorizerBase<>).MakeGenericType(typeof(CreateOrangeCommand));
        private readonly Type commandValidatorType = typeof(Mock.Dispatcher.Validation.CommandValidatorBase<>).MakeGenericType(typeof(CreateOrangeCommand));
        private readonly Type commandHandlerType = typeof(Mock.Dispatcher.ICommandHandler<>).MakeGenericType(typeof(CreateOrangeCommand));

        private Mock<Mock.Dispatcher.IIocContainer> iocContainerMock;
        private Mock<Mock.Dispatcher.Authorization.ICommandAuthorizer<CreateOrangeCommand>> commandAuthorizerMock;
        private Mock<Mock.Dispatcher.Validation.ICommandValidator<CreateOrangeCommand>> commandValidatorMock;
        private Mock<Mock.Dispatcher.ICommandHandler<CreateOrangeCommand>> commandHandlerMock;

        private Mock.Dispatcher.CommandDispatcher commandDispatcher;

        [TestInitialize]
        public void TestInitialize()
        {
            iocContainerMock = new Mock<Mock.Dispatcher.IIocContainer>(MockBehavior.Strict);
            commandAuthorizerMock = new Mock<Mock.Dispatcher.Authorization.ICommandAuthorizer<CreateOrangeCommand>>(MockBehavior.Strict);
            commandValidatorMock = new Mock<Mock.Dispatcher.Validation.ICommandValidator<CreateOrangeCommand>>(MockBehavior.Strict);
            commandHandlerMock = new Mock<Mock.Dispatcher.ICommandHandler<CreateOrangeCommand>>(MockBehavior.Strict);

            commandDispatcher = new Mock.Dispatcher.CommandDispatcher(iocContainerMock.Object);
        }

        [TestMethod]
        public void CommandDispatcher_Dispatch_Unauthorized_ReturnReponse()
        {
            Mock.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateOrangeCommand));

            CreateOrangeCommand createOrangeCommand = new CreateOrangeCommand();

            iocContainerMock
                .Setup(x => x.GetInstance(It.Is<Type>(y => y == commandAuthorizerType)))
                .Returns(commandAuthorizerMock.Object);

            commandAuthorizerMock
                .Setup(x => x.Authorize(createOrangeCommand))
                .Returns(false);

            Mock.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createOrangeCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }

        [TestMethod]
        public void CommandDispatcher_Dispatch_Invalid_ReturnReponse()
        {
            Mock.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateOrangeCommand), isAuthorized: true);

            expectedCommandResponse.AddValidationError(field: "field", message: "message");

            CreateOrangeCommand createOrangeCommand = new CreateOrangeCommand();

            iocContainerMock
                .Setup(x => x.GetInstance(It.Is<Type>(y => y == commandAuthorizerType)))
                .Returns(commandAuthorizerMock.Object);

            commandAuthorizerMock
                .Setup(x => x.Authorize(createOrangeCommand))
                .Returns(true);

            iocContainerMock
                .Setup(x => x.GetInstance(It.Is<Type>(y => y == commandValidatorType)))
                .Returns(commandValidatorMock.Object);

            commandValidatorMock
                .Setup(x => x.Validate(createOrangeCommand))
                .Returns(new List<Mock.Dispatcher.Validation.ValidationError>() { new Mock.Dispatcher.Validation.ValidationError() { Field = "field", Message = "message" } });

            Mock.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createOrangeCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }

        [TestMethod]
        public void CommandDispatcher_Dispatch_ReturnReponse()
        {
            Mock.Dispatcher.CommandResponse expectedCommandResponse = UnitTestHelper.CreateCommandReponse(commandType: typeof(CreateOrangeCommand), isAuthorized: true, errors: new List<Mock.Dispatcher.Validation.ValidationError>());

            expectedCommandResponse.AddContent(new CreateOrangeCommandResponse() { Name = "Create orange command response!" });

            CreateOrangeCommand createOrangeCommand = new CreateOrangeCommand();

            iocContainerMock
                .Setup(x => x.GetInstance(It.Is<Type>(y => y == commandAuthorizerType)))
                .Returns(commandAuthorizerMock.Object);

            commandAuthorizerMock
                .Setup(x => x.Authorize(createOrangeCommand))
                .Returns(true);

            iocContainerMock
                .Setup(x => x.GetInstance(It.Is<Type>(y => y == commandValidatorType)))
                .Returns(commandValidatorMock.Object);

            commandValidatorMock
                .Setup(x => x.Validate(createOrangeCommand))
                .Returns(new List<Mock.Dispatcher.Validation.ValidationError>());

            iocContainerMock
                .Setup(x => x.GetInstance(It.Is<Type>(y => y == commandHandlerType)))
                .Returns(commandHandlerMock.Object);

            commandHandlerMock
                .Setup(x => x.Execute(createOrangeCommand))
                .Returns(new CreateOrangeCommandResponse() { Name = "Create orange command response!" });

            Mock.Dispatcher.ICommandResponse actualCommandResponse = commandDispatcher.Dispatch(createOrangeCommand);

            RagnarAssert.AreEqual(expectedCommandResponse, actualCommandResponse);
        }
    }

    public class CreateOrangeCommand : Mock.Dispatcher.ICommand
    {

    }

    public class CreateOrangeCommandResponse
    {
        public string Name { get; set; }
    }
}