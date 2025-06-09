using System.Text;
using MalshinonApp.Data;
using MalshinonApp.Models;

namespace MalshinonApp.Services;

public class CSVExportService
{
    private readonly IReportRepository _reportRepository;

    public CSVExportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public void ExportReportsToCsv(string filePath)
    {
        var reports = _reportRepository.GetAllReports();

        var lines = new List<string>
        {
            "ReportId,ReporterId,TargetId,Text,Timestamp"
        };

        foreach (var r in reports)
        {
            var line = $"{r.Id},{r.ReporterId},{r.TargetId},\"{r.Text.Replace("\"", "\"\"")}\",{r.Timestamp:o}";
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines, Encoding.UTF8);
        Console.WriteLine($"Reports exported to {filePath}");
    }
}