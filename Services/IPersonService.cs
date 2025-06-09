using MalshinonApp.Models;

namespace MalshinonApp.Services
{
    public interface IPersonService
    {
        Person GetOrCreateByName(string fullName);
        Person GetOrCreateByCode(string secretCode);
        string GetSecretCodeByName(string fullName);
    }
}