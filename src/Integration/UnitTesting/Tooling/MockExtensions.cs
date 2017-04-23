using Moq;
using System;
using System.Collections.Generic;

namespace Ragnar.Integration.UnitTesting.Tooling
{
    public static class MockExtensions
    {
        private delegate void Verifiable();

        private static List<Verifiable> verifiables = new List<Verifiable>();

        public static void ResetVerifiables()
        {
            verifiables = new List<Verifiable>();
        }

        public static void VerifyAll()
        {
            verifiables.ForEach(verifiable => verifiable());
        }

        public static void Setup_Save(this Mock<Integration.Tooling.IPersonRepository> personRepository, Integration.Tooling.Person person)
        {
            personRepository.Setup(x => x.Save(It.Is<Integration.Tooling.Person>(y => RagnarAssert.Match(person, y))));

            verifiables.Add(() => personRepository.Verify(x => x.Save(It.Is<Integration.Tooling.Person>(y => RagnarAssert.Match(person, y))), Times.AtMostOnce()));
        }

        public static void Setup_DateTime(this Mock<Integration.Tooling.Utilities.IDateTimeProvider> dateTimeProvider, DateTime dateTime)
        {
            dateTimeProvider.Setup(x => x.DateTime()).Returns(dateTime);

            verifiables.Add(() => dateTimeProvider.Verify(x => x.DateTime(), Times.AtMostOnce()));
        }
    }
}
