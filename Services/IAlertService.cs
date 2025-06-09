using MalshinonApp.Models;

namespace MalshinonApp.Services
{
    public interface IAlertService
    {
        void EvaluateAlertsForTarget(int targetId, DateTime newReportTime);
        List<Alert> GetAllAlerts();
    }
}