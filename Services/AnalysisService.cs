using MalshinonApp.Models;
using MalshinonApp.Data;

namespace MalshinonApp.Services
{
    public class AnalysisService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPersonRepository _personRepository;

        public AnalysisService(IReportRepository reportRepo, IPersonRepository personRepo)
        {
            _reportRepository = reportRepo;
            _personRepository = personRepo;
        }

        public List<Person> GetPotentialRecruits()
        {
            var result = new List<Person>();
            var allPeople = _personRepository.GetAll();

            foreach (var person in allPeople)
            {
                var reports = _reportRepository.GetReportsByReporterId(person.Id);
                if (reports.Count >= 10)
                {
                    double avgLength = reports.Average(r => r.ReportText.Length);
                    if (avgLength >= 100)
                    {
                        result.Add(person);
                    }
                }
            }

            return result;
        }

        public List<Person> GetHighRiskTargets()
        {
            var result = new List<Person>();
            var allPeople = _personRepository.GetAll();

            foreach (var person in allPeople)
            {
                var reports = _reportRepository.GetReportsByTargetId(person.Id);

                if (reports.Count >= 20 || HasBurst(reports))
                {
                    result.Add(person);
                }
            }

            return result;
        }

        private bool HasBurst(List<Report> reports)
        {
            var sorted = reports.OrderBy(r => r.Timestamp).ToList();

            for (int i = 0; i < sorted.Count - 2; i++)
            {
                DateTime start = sorted[i].Timestamp;
                DateTime end = sorted[i + 2].Timestamp;

                if ((end - start).TotalMinutes <= 15)
                    return true;
            }

            return false;
        }
    }
}