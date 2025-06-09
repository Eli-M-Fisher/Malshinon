using MalshinonApp.Models;
using MalshinonApp.Data;

namespace MalshinonApp.Services
{
    public class ReportService : IReportService
    {
        private readonly IPersonService _personService;
        private readonly IReportRepository _reportRepository;

        public ReportService(IPersonService personService, IReportRepository reportRepository)
        {
            _personService = personService;
            _reportRepository = reportRepository;
        }

        public void SubmitReport(string reporterIdentifier, string targetIdentifier, string text, DateTime timestamp)
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
        }

        private Person GetOrCreatePerson(string identifier)
        {
            // נסה לפי קוד
            if (identifier.Length == 8 && identifier.All(char.IsLetterOrDigit))
            {
                return _personService.GetOrCreateByCode(identifier);
            }

            // אחרת - לפי שם
            return _personService.GetOrCreateByName(identifier);
        }
    }
}