using Ragnar.Integration.Tooling.Utilities;
using System;

namespace Ragnar.Integration.Tooling
{
    public class PersonService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IPersonRepository personRepository;

        public PersonService(
            IDateTimeProvider dateTimeProvider,
            IPersonRepository personRepository)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.personRepository = personRepository;
        }

        public void Save(Person person)
        {
            if (person.ID == Guid.Empty)
            {
                person.ID = RagnarGuid.NewGuid();
                person.RegistrationDate = dateTimeProvider.DateTime();
            }

            personRepository.Save(person);
        }
    }
}