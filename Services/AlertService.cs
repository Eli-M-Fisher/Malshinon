using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services.Logging;

namespace MalshinonApp.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IReportRepository _reportRepository;

        public AlertService(IAlertRepository alertRepo, IReportRepository reportRepo)
        {
            _alertRepository = alertRepo;
            _reportRepository = reportRepo;
        }

        public void EvaluateAlertsForTarget(int targetId, DateTime newReportTime)
        {
            var reports = _reportRepository.GetReportsByTargetId(targetId)
                                           .OrderBy(r => r.Timestamp)
                                           .ToList();

            // בדוק אם יש פיק חדש
            for (int i = 0; i < reports.Count - 2; i++)
            {
                DateTime start = reports[i].Timestamp;
                DateTime end = reports[i + 2].Timestamp;

                if ((end - start).TotalMinutes <= 15)
                {
                    if (!AlertExists(targetId, start, end))
                    {
                        CreateAlert(targetId, start, end, "Burst of reports (3+) within 15 minutes");
                    }
                }
            }

            // בדוק אם עבר את סף 20 דיווחים
            if (reports.Count >= 20)
            {
                var existing = _alertRepository.GetAlertsByTargetId(targetId);
                if (!existing.Any(a => a.Reason.Contains("20 reports")))
                {
                    CreateAlert(targetId, reports.First().Timestamp, reports.Last().Timestamp,
                        "Target mentioned in 20+ reports");
                }
            }
            SimpleLogger.Log("Info", $"Evaluated alerts for Target #{targetId} at {DateTime.Now}");
        }

        private bool AlertExists(int targetId, DateTime start, DateTime end)
        {
            var alerts = _alertRepository.GetAlertsByTargetId(targetId);
            return alerts.Any(a =>
                a.AlertTimeWindowStart == start &&
                a.AlertTimeWindowEnd == end);
        }

        private void CreateAlert(int targetId, DateTime start, DateTime end, string reason)
        {
            var alert = new Alert
            {
                TargetId = targetId,
                AlertTimeWindowStart = start,
                AlertTimeWindowEnd = end,
                Reason = reason,
                CreatedAt = DateTime.Now
            };

            _alertRepository.Add(alert);
            SimpleLogger.Log("Alert", $"New alert created for Target #{targetId} | Reason: {reason} | Window: {start}–{end}");
        }

        public List<Alert> GetAllAlerts()
        {
            return _alertRepository.GetAll();
        }
    }
}