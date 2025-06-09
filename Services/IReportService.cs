using MalshinonApp.Models;

namespace MalshinonApp.Services
{
    public interface IReportService
    {
        void SubmitReport(string reporterIdentifier, string targetIdentifier, string text, DateTime timestamp);
    }
}