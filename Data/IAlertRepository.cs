using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public interface IAlertRepository
    {
        void Add(Alert alert);
        List<Alert> GetAlertsByTargetId(int targetId);
        List<Alert> GetAll();
    }
}