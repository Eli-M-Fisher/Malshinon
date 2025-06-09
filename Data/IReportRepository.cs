using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public interface IReportRepository
    {
        Report? GetById(int id);
        void Add(Report report);
        List<Report> GetReportsByTargetId(int targetId);
        List<Report> GetReportsByReporterId(int reporterId);
        List<Report> GetAll();
        List<Report> GetAllReports();
    }
}