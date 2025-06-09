using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Utils;

namespace MalshinonApp.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Person GetOrCreateByName(string fullName)
        {
            var person = _personRepository.GetByFullName(fullName);
            if (person != null)
                return person;

            var newPerson = new Person
            {
                FullName = fullName,
                SecretCode = SecretCodeGenerator.Generate(),
                CreatedAt = DateTime.Now
            };
            _personRepository.Add(newPerson);
            return newPerson;
        }

        public Person GetOrCreateByCode(string secretCode)
        {
            var person = _personRepository.GetBySecretCode(secretCode);
            if (person != null)
                return person;

            var newPerson = new Person
            {
                FullName = "Unknown",
                SecretCode = secretCode,
                CreatedAt = DateTime.Now
            };
            _personRepository.Add(newPerson);
            return newPerson;
        }

        public string GetSecretCodeByName(string fullName)
        {
            var person = _personRepository.GetByFullName(fullName);
            if (person == null)
                throw new Exception($"Person with name '{fullName}' not found.");
            return person.SecretCode;
        }
    }
}