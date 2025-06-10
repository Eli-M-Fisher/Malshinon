using System.Globalization;
using System.Text;
using MalshinonApp.Models;
using MalshinonApp.Data;
using Microsoft.VisualBasic.FileIO;
using MalshinonApp.Services.Logging;

namespace MalshinonApp.Services;

public class CSVImportService : ICSVImportService
{
    private readonly IReportRepository _reportRepository;

    public CSVImportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public List<Report> ImportReportsFromCsv(string filePath)
    {
        var importedReports = new List<Report>();

        if (!File.Exists(filePath))
        {
            SimpleLogger.Log("ERROR", $"File '{filePath}' does not exist.");
            return importedReports;
        }

        using var parser = new TextFieldParser(filePath, Encoding.UTF8);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        parser.ReadLine(); // Skip header

        while (!parser.EndOfData)
        {
            try
            {
                string[]? fields = parser.ReadFields();
                SimpleLogger.Log("DEBUG", $"line: {string.Join(" | ", fields)}");

                if (fields == null || fields.Length < 5)
                {
                    SimpleLogger.Log("WARN", "Skipping invalid line.");
                    continue;
                }

                var report = new Report
                {
                    Id = int.Parse(fields[0]),
                    ReporterId = int.Parse(fields[1]),
                    TargetId = int.Parse(fields[2]),
                    ReportText = fields[3],
                    Timestamp = DateTime.Parse(fields[4], null, DateTimeStyles.RoundtripKind)
                };

                _reportRepository.Add(report);
                importedReports.Add(report);
            }
            catch (Exception ex)
            {
                SimpleLogger.Log("ERROR", $"Importing line: {ex.Message}");
            }
        }

        SimpleLogger.Log("INFO", "Reports imported successfully.");
        return importedReports;
    }
}