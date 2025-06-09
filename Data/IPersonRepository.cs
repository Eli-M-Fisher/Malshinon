using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public interface IPersonRepository
    {
        Person? GetById(int id);
        Person? GetBySecretCode(string code);
        Person? GetByFullName(string name);
        void Add(Person person);
        void Update(Person person);
        List<Person> GetAll();
    }
}