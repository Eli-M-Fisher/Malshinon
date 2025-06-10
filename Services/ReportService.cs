using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services.Logging;

namespace MalshinonApp.Services
{
    public class ReportService : IReportService
    {
        private readonly IPersonService _personService;
        private readonly IReportRepository _reportRepository;
        private readonly IAlertService _alertService;

        public ReportService(
            IPersonService personService,
            IReportRepository reportRepository,
            IAlertService alertService)
        {
            _personService = personService;
            _reportRepository = reportRepository;
            _alertService = alertService;
        }

        public void SubmitReport(string reporterIdentifier, string targetIdentifier, string text, DateTime timestamp)
        {
            try
            {
                Person reporter = GetOrCreatePerson(reporterIdentifier);
                Person target = GetOrCreatePerson(targetIdentifier);

                var report = new Report
                {
                    ReporterId = reporter.Id,
                    TargetId = target.Id,
                    ReportText = text,
                    Timestamp = timestamp
                };

                _reportRepository.Add(report);
                SimpleLogger.LogInfo($"Report submitted by '{reporterIdentifier}' against '{targetIdentifier}' at {timestamp}.");

                _alertService.EvaluateAlertsForTarget(target.Id, timestamp);
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error in SubmitReport: {ex.Message}");
                throw;
            }
        }

        private Person GetOrCreatePerson(string identifier)
        {
            if (identifier.Length == 8 && identifier.All(char.IsLetterOrDigit))
            {
                return _personService.GetOrCreateByCode(identifier);
            }

            return _personService.GetOrCreateByName(identifier);
        }
    }
}