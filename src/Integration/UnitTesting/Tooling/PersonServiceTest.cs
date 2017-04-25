using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ragnar.Integration.UnitTesting.Tooling
{
    [TestClass]
    public class PersonServiceTest
    {
        private Mock<Integration.Tooling.Utilities.IDateTimeProvider> dateTimeProviderMock;
        private Mock<Integration.Tooling.IPersonRepository> personRepositoryMock;

        private Integration.Tooling.PersonService personService;

        [TestInitialize]
        public void TestInitialize()
        {
            dateTimeProviderMock = new Mock<Integration.Tooling.Utilities.IDateTimeProvider>(MockBehavior.Strict);
            personRepositoryMock = new Mock<Integration.Tooling.IPersonRepository>(MockBehavior.Strict);

            personService = new Integration.Tooling.PersonService(
                dateTimeProvider: dateTimeProviderMock.Object,
                personRepository: personRepositoryMock.Object);

            Integration.Tooling.Utilities.RagnarGuid.GuidProvider = new UnitTestHelper.GuidProvider(new[] { UnitTestHelper.guid1, UnitTestHelper.guid2, UnitTestHelper.guid3 });

            MockExtensions.ResetVerifiables();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            MockExtensions.VerifyAll();
        }

        [TestMethod]
        public void PersonService_Save_Success()
        {
            Integration.Tooling.Person person = UnitTestHelper.CreatePerson();

            Integration.Tooling.Person expectedPerson = UnitTestHelper.CreatePerson(id: UnitTestHelper.guid1, name: "Ragnar", registrationDate: new DateTime(2017, 01, 01));

            dateTimeProviderMock.Setup_DateTime(new DateTime(2017, 01, 01));
            personRepositoryMock.Setup_Save(expectedPerson);

            personService.Save(person);
        }
    }
}
