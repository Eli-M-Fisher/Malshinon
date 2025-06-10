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
        if (_reportRepository == null)
        {
            Console.WriteLine("Error: Report repository is not initialized.");
            return;
        }

        var reports = _reportRepository.GetAllReports();

        var lines = new List<string>
        {
            "ReportId,ReporterId,TargetId,Text,Timestamp"
        };

        foreach (var r in reports)
        {
            if (r == null)
            {
                Console.WriteLine("⚠️ Skipping null report");
                continue;
            }

            string text = r.ReportText ?? "(no text)";
            var line = $"{r.Id},{r.ReporterId},{r.TargetId},\"{text.Replace("\"", "\"\"")}\",{r.Timestamp:o}";
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines, Encoding.UTF8);
        Console.WriteLine($"Reports exported to {filePath}");
    }
}