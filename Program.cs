using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services;

var personRepo = new PersonRepository();
var reportRepo = new ReportRepository();
var alertRepo = new AlertRepository();

var personService = new PersonService(personRepo);
var alertService = new AlertService(alertRepo, reportRepo);
var reportService = new ReportService(personService, reportRepo, alertService);
var analysisService = new AnalysisService(reportRepo, personRepo);

while (true)
{
    Console.WriteLine("\n=== Malshinon Intelligence System ===");
    Console.WriteLine("1. Submit a Report");
    Console.WriteLine("2. Show Potential Recruits");
    Console.WriteLine("3. Show High-Risk Targets");
    Console.WriteLine("4. Show Alerts");
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

            reportService.SubmitReport(reporterInput, targetInput, text, now);
            Console.WriteLine("Report submitted successfully!");
            break;

        case "2":
            Console.WriteLine("\nPotential Recruits:");
            var recruits = analysisService.GetPotentialRecruits();
            foreach (var p in recruits)
            {
                Console.WriteLine($"- {p.FullName} ({p.SecretCode})");
            }
            break;

        case "3":
            Console.WriteLine("\nHigh-Risk Targets:");
            var threats = analysisService.GetHighRiskTargets();
            foreach (var p in threats)
            {
                Console.WriteLine($"- {p.FullName} ({p.SecretCode})");
            }
            break;

        case "4":
            Console.WriteLine("\nAlerts:");
            var alerts = alertService.GetAllAlerts();
            foreach (var a in alerts)
            {
                Console.WriteLine($"- Target #{a.TargetId} | {a.Reason} | {a.AlertTimeWindowStart}–{a.AlertTimeWindowEnd}");
            }
            break;

        case "0":
            Console.WriteLine("Exiting...");
            return;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}