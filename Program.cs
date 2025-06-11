using MalshinonApp.Services.Logging;
using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services;

var personRepo = new PersonRepository();
Console.WriteLine(personRepo == null ? "❌ personRepo is NULL" : "✅ personRepo is OK");
var connection = DbConnectionHelper.GetConnection();
Console.WriteLine(connection == null ? "❌ connection is NULL" : "✅ connection is OK");
var reportRepo = new ReportRepository(connection!);
Console.WriteLine(reportRepo == null ? "❌ reportRepo is NULL" : "✅ reportRepo is OK");
var alertRepo = new AlertRepository();
Console.WriteLine(alertRepo == null ? "❌ alertRepo is NULL" : "✅ alertRepo is OK");

var personService = new PersonService(personRepo!);
Console.WriteLine(personService == null ? "❌ personService is NULL" : "✅ personService is OK");
var alertService = new AlertService(alertRepo!, reportRepo!);
Console.WriteLine(alertService == null ? "❌ alertService is NULL" : "✅ alertService is OK");
var reportService = new ReportService(personService!, reportRepo!, alertService!);
Console.WriteLine(reportService == null ? "❌ reportService is NULL" : "✅ reportService is OK");
var analysisService = new AnalysisService(reportRepo, personRepo);
Console.WriteLine(analysisService == null ? "❌ analysisService is NULL" : "✅ analysisService is OK");
var csvExportService = new CSVExportService(reportRepo);
Console.WriteLine(csvExportService == null ? "❌ csvExportService is NULL" : "✅ csvExportService is OK");
var csvImportService = new CSVImportService(reportRepo);
Console.WriteLine(csvImportService == null ? "❌ csvImportService is NULL" : "✅ csvImportService is OK");

// Simple logging usage without singleton

while (true)
{
    Console.WriteLine("\n=== Malshinon Intelligence System ===");
    Console.WriteLine("1. Submit a Report");
    Console.WriteLine("2. Show Dashboard");
    Console.WriteLine("3. Export Reports to CSV");
    Console.WriteLine("4. Import Reports from CSV");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    string choice = Console.ReadLine()?.Trim() ?? "";

    switch (choice)
    {
        case "1":
            Console.Write("Enter your identifier (name or secret code): ");
            string reporterInput = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter target identifier (name or secret code): ");
            string targetInput = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter report text: ");
            string text = Console.ReadLine()?.Trim() ?? "";

            DateTime now = DateTime.Now;

            reportService!.SubmitReport(reporterInput, targetInput, text, now);
            SimpleLogger.Log($"Report submitted by '{reporterInput}' against '{targetInput}' at {now}.", "Reports");
            Console.WriteLine("Report submitted successfully!");
            break;

        case "2":
            Console.WriteLine("\n=== Analysis Dashboard ===");

            Console.WriteLine("\nPotential Recruits:");
            var recruits = analysisService!.GetPotentialRecruits();
            foreach (var p in recruits)
            {
                Console.WriteLine($"- {p.FullName} ({p.SecretCode})");
            }

            Console.WriteLine("\nHigh-Risk Targets:");
            var threats = analysisService!.GetHighRiskTargets();
            foreach (var p in threats)
            {
                Console.WriteLine($"- {p.FullName} ({p.SecretCode})");
            }

            Console.WriteLine("\nAlerts:");
            var alerts = alertService!.GetAllAlerts();
            foreach (var a in alerts)
            {
                Console.WriteLine($"- Target #{a.TargetId} | {a.Reason} | {a.AlertTimeWindowStart}–{a.AlertTimeWindowEnd}");
            }

            Console.WriteLine("\nReports:");
            var reports = reportRepo!.GetAllReports();
            foreach (var r in reports)
            {
                Console.WriteLine($"- {r.Timestamp:g} | From #{r.ReporterId} -> #{r.TargetId}: {r.ReportText}");
            }

            break;

        case "3":
            Console.Write("Enter file path for CSV export (e.g., reports.csv): ");
            string path = Console.ReadLine()?.Trim() ?? "";
            csvExportService!.ExportReportsToCsv(path);
            SimpleLogger.Log($"Reports exported to '{path}'.", "Export");
            break;

        case "4":
            Console.Write("Enter CSV file path (e.g., reports.csv or CSV/reports.csv): ");
            string importPath = Console.ReadLine()?.Trim() ?? "";
            var importedReports = csvImportService!.ImportReportsFromCsv(importPath);
            Console.WriteLine($"Imported {importedReports.Count} reports successfully.");
            SimpleLogger.Log($"{importedReports.Count} reports imported from '{importPath}'.", "Import");
            break;

        case "0":
            Console.WriteLine("Exiting...");
            SimpleLogger.Log("Application exited.", "System");
            return;

        default:
            Console.WriteLine("Invalid option.");
            SimpleLogger.Log($"Invalid menu option selected: '{choice}'.", "Input");
            break;
    }
}